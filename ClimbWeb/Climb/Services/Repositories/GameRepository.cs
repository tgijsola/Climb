using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext dbContext;

        public GameRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<Game>> ListAll()
        {
            return dbContext.Games.ToListAsync();
        }

        public Task<bool> AnyExist(string name)
        {
            return dbContext.Games.AnyAsync(g => g.Name == name);
        }

        public async Task<Game> Create(string name)
        {
            var game = new Game {Name = name};

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return game;
        }

        public async Task<bool> Any(Expression<Func<Game, bool>> predicate)
        {
            return await dbContext.Games.AnyAsync(predicate);
        }
    }
}