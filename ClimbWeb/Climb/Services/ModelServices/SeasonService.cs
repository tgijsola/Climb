using System;
using System.Linq;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.ModelServices
{
    [UsedImplicitly]
    public class SeasonService : ISeasonService
    {
        private readonly ApplicationDbContext dbContext;

        public SeasonService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Season> Create(int leagueID, DateTime start, DateTime end)
        {
            var seasonCount = await dbContext.Seasons.CountAsync(s => s.LeagueID == leagueID);

            var season = new Season(leagueID, seasonCount, start, end);
            dbContext.Add(season);

            var league = await dbContext.Leagues
                .Include(l => l.Members).AsNoTracking()
                .FirstAsync(l => l.ID == leagueID);
            var participants = league.Members.Select(lu => new SeasonLeagueUser(season.ID, lu.ID));
            dbContext.AddRange(participants);

            await dbContext.SaveChangesAsync();

            return season;
        }

        public async Task GenerateSchedule(int seasonID)
        {
            var season = await dbContext.Seasons
                .Include(s => s.Sets).AsNoTracking()
                .Include(s => s.Participants).AsNoTracking()
                .FirstAsync(s => s.ID == seasonID);

            if(season.Participants.Count < 2)
            {
                throw new InvalidOperationException("Season needs more than 2 participants.");
            }

            var participants = season.Participants.ToArray();
            for(int i = 0; i < participants.Length - 1; i++)
            {
                var player1 = participants[i];
                for(int j = i + 1; j < participants.Length; j++)
                {
                    var player2 = participants[j];
                    var dueDate = DateTime.Now;
                    var set = new Set(season.LeagueID, season.ID, player1.LeagueUserID, player2.LeagueUserID, dueDate);
                    season.Sets.Add(set);
                }
            }

            dbContext.Sets.AddRange(season.Sets);
            await dbContext.SaveChangesAsync();
        }
    }
}