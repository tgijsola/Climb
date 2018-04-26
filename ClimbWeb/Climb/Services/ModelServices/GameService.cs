using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;

namespace Climb.Services.ModelServices
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext dbContext;

        public GameService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Game> Create(string name)
        {
            var game = new Game {Name = name};

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return game;
        }
    }
}