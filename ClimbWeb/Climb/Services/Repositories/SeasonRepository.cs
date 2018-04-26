using System;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.Repositories
{
    [UsedImplicitly]
    public class SeasonRepository : DbRepository<Season>, ISeasonRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SeasonRepository(ApplicationDbContext dbContext)
            : base(dbContext.Seasons)
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

            dbSet.Add(season);

            await dbContext.SaveChangesAsync();

            return season;
        }
    }
}