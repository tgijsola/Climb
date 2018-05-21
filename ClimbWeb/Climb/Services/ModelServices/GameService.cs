using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Models;
using Climb.Requests.Games;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.ModelServices
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext dbContext;

        public GameService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Game> Create(CreateRequest request)
        {
            if(request.CharactersPerMatch < 1)
            {
                throw new BadRequestException(nameof(request.CharactersPerMatch), "A game needs at least 1 character per match");
            }

            if(request.MaxMatchPoints < 1)
            {
                throw new BadRequestException(nameof(request.MaxMatchPoints), "A game needs at least 1 point per match.");
            }

            if(await dbContext.Games.AnyAsync(g => g.Name == request.Name))
            {
                throw new BadRequestException(nameof(request.Name), $"A game with the name '{request.Name}' already exists.");
            }

            var game = new Game(request.Name, request.CharactersPerMatch, request.MaxMatchPoints);

            dbContext.Add(game);
            await dbContext.SaveChangesAsync();

            return game;
        }
    }
}