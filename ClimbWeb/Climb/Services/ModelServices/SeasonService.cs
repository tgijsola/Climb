using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.ModelServices
{
    [UsedImplicitly]
    public class SeasonService : ISeasonService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ScheduleFactory scheduleFactory;

        public SeasonService(ApplicationDbContext dbContext, ScheduleFactory scheduleFactory)
        {
            this.dbContext = dbContext;
            this.scheduleFactory = scheduleFactory;
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

        public async Task<HashSet<Set>> GenerateSchedule(int seasonID)
        {
            var season = await dbContext.Seasons
                .Include(s => s.Sets)
                .Include(s => s.Participants).AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == seasonID);
            if(season == null)
            {
                throw new NotFoundException(typeof(Season), seasonID);
            }

            var sets = await scheduleFactory.GenerateScheduleAsync(season, dbContext);

            return sets;
        }
    }
}