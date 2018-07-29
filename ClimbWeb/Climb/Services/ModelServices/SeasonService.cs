using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Climb.Core.TieBreakers;
using Climb.Data;
using Climb.Exceptions;
using Climb.Models;
using Microsoft.EntityFrameworkCore;
using ITieBreaker = Climb.Core.TieBreakers.ITieBreaker;
using ITieBreakerFactory = Climb.Core.TieBreakers.ITieBreakerFactory;

namespace Climb.Services.ModelServices
{
    public class SeasonService : ISeasonService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IScheduleFactory scheduleFactory;
        private readonly ISeasonPointCalculator pointCalculator;
        private readonly ITieBreaker tieBreaker;

        public SeasonService(ApplicationDbContext dbContext, IScheduleFactory scheduleFactory, ISeasonPointCalculator pointCalculator, ITieBreakerFactory tieBreakerFactory)
        {
            this.dbContext = dbContext;
            this.scheduleFactory = scheduleFactory;
            this.pointCalculator = pointCalculator;

            tieBreaker = tieBreakerFactory.Create();
        }

        public async Task<Season> Create(int leagueID, DateTime start, DateTime end)
        {
            if(start < DateTime.Now)
            {
                throw new BadRequestException(nameof(start), "Start date can't be in the past.");
            }

            if(end <= start)
            {
                throw new BadRequestException(nameof(end), "End date must be after the start date.");
            }

            var league = await dbContext.Leagues
                .Include(l => l.Members).AsNoTracking()
                .FirstOrDefaultAsync(l => l.ID == leagueID);
            if(league == null)
            {
                throw new NotFoundException(typeof(League), leagueID);
            }

            var seasonCount = await dbContext.Seasons.CountAsync(s => s.LeagueID == leagueID);

            var season = new Season(leagueID, seasonCount, start, end);
            dbContext.Add(season);

            var participants = league.Members.Select(lu => new SeasonLeagueUser(season.ID, lu.ID));
            dbContext.AddRange(participants);

            await dbContext.SaveChangesAsync();

            return season;
        }

        public async Task<Season> GenerateSchedule(int seasonID)
        {
            var season = await dbContext.Seasons
                .Include(s => s.Sets)
                .Include(s => s.Participants).AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == seasonID);
            if(season == null)
            {
                throw new NotFoundException(typeof(Season), seasonID);
            }

            await scheduleFactory.GenerateScheduleAsync(season, dbContext);

            return season;
        }

        public async Task<Season> UpdateStandings(int setID)
        {
            var set = await dbContext.Sets
                .Include(s => s.Season).ThenInclude(s => s.Participants)
                .Include(s => s.Season).ThenInclude(s => s.Sets).AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == setID);
            dbContext.Update(set);

            UpdatePoints(set);
            BreakTies(set.Season);
            UpdateRanks(set.Season);

            await dbContext.SaveChangesAsync();

            return set.Season;
        }

        private void UpdatePoints(Set set)
        {
            var winner = set.WinnerID == set.Player1ID ? set.SeasonPlayer1 : set.SeasonPlayer2;
            dbContext.Update(winner);
            var loser = set.LoserID == set.Player1ID ? set.SeasonPlayer1 : set.SeasonPlayer2;
            dbContext.Update(loser);

            var (winnerPointDelta, loserPointDelta) = pointCalculator.CalculatePointDeltas(winner, loser);

            set.Player1SeasonPoints = set.WinnerID == set.Player1ID ? winnerPointDelta : loserPointDelta;
            set.Player2SeasonPoints = set.WinnerID == set.Player2ID ? winnerPointDelta : loserPointDelta;
            winner.Points += winnerPointDelta;
            loser.Points += loserPointDelta;
        }

        private void BreakTies(Season season)
        {
            var playedSets = season.Sets.Where(s => s.IsComplete).ToArray();
            var setWins = new Dictionary<int, List<int>>();
            foreach(var participant in season.Participants)
            {
                setWins.Add(participant.ID, new List<int>());
            }

            foreach(var set in playedSets)
            {
                Debug.Assert(set.WinnerID != null, "set.WinnerID != null");
                Debug.Assert(set.LoserID != null, "set.LoserID != null");
                setWins[set.WinnerID.Value].Add(set.LoserID.Value);
            }

            season.Participants.Sort();

            var currentPoints = -1;
            var tiedParticipants = new Dictionary<IParticipant, ParticipantRecord>();
            foreach(var seasonLeagueUser in season.Participants)
            {
                if(seasonLeagueUser.Points != currentPoints)
                {
                    if(tiedParticipants.Count > 1)
                    {
                        foreach(var participantRecord in tiedParticipants)
                        {
                            participantRecord.Value.AddWins(setWins[participantRecord.Key.ID]);
                        }
                    }

                    tieBreaker.Break(tiedParticipants);
                }

                currentPoints = seasonLeagueUser.Points;
                tiedParticipants.Clear();
                var record = new ParticipantRecord(seasonLeagueUser.LeagueUser.Points, seasonLeagueUser.LeagueUser.JoinDate);
                tiedParticipants.Add(seasonLeagueUser, record);
            }
        }

        private void UpdateRanks(Season season)
        {
            dbContext.UpdateRange(season.Participants);

            var sortedParticipants = season.Participants.OrderByDescending(slu => slu.Points).ToArray();

            var rank = 1;
            var lastPoints = -1;
            for(var i = 0; i < sortedParticipants.Length; i++)
            {
                var participant = sortedParticipants[i];
                participant.Standing = rank;
                if(participant.Points != lastPoints)
                {
                    lastPoints = participant.Points;
                }

                ++rank;
            }
        }
    }
}