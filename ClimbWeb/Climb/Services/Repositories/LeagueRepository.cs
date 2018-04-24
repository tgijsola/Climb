using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly ApplicationDbContext dbContext;

        public LeagueRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<League>> ListAll()
        {
            return dbContext.Leagues.ToListAsync();
        }

        public async Task<League> Create(string name, int gameID)
        {
            var league = new League
            {
                Name = name,
                GameID = gameID,
            };

            dbContext.Leagues.Add(league);
            await dbContext.SaveChangesAsync();

            return league;
        }

        public async Task<bool> Any(Expression<Func<League, bool>> predicate)
        {
            return await dbContext.Leagues.AnyAsync(predicate);
        }
    }
}