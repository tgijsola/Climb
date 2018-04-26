using System;
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

            var season = new Season
            {
                LeagueID = leagueID,
                StartDate = start,
                EndDate = end,
                Index = seasonCount,
            };

            dbContext.Add(season);

            await dbContext.SaveChangesAsync();

            return season;
        }
    }
}