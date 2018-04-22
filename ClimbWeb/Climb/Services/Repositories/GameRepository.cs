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

        public Task<bool> AnyExist(string name)
        {
            return dbContext.Game.AnyAsync(g => g.Name == name);
        }

        public async Task<Game> Create(string name)
        {
            var game = new Game {Name = name};

            dbContext.Game.Add(game);
            await dbContext.SaveChangesAsync();

            return game;
        }
    }
}