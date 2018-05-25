using System.Linq;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Models;
using Microsoft.EntityFrameworkCore;
using Climb.Requests.Games;

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
            if(await dbContext.Games.AnyAsync(g => g.Name == request.Name))
            {
                throw new BadRequestException();
            }

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

        public async Task<Character> AddCharacter(AddCharacterRequest request)
        {
            var game = await dbContext.Games
                .Include(g => g.Characters).AsNoTracking()
                .FirstOrDefaultAsync(g => g.ID == request.GameID);
            if(game == null)
            {
                throw new NotFoundException(typeof(Game), request.GameID);
            }

            if(game.Characters.Any(c => c.Name == request.Name))
            {
                throw new BadRequestException(nameof(request.Name), $"There is already a character with the name '{request.Name}'.");
            }

            var character = new Character
            {
                Name = request.Name,
                GameID = request.GameID,
            };

            dbContext.Add(character);
            await dbContext.SaveChangesAsync();

            return character;
        }

        public async Task<Stage> AddStage(AddStageRequest request)
        {
            var game = await dbContext.Games
                .Include(g => g.Stages).AsNoTracking()
                .FirstOrDefaultAsync(g => g.ID == request.GameID);
            if(game == null)
            {
                throw new NotFoundException(typeof(Game), request.GameID);
            }

            if(game.Stages.Any(c => c.Name == request.Name))
            {
                throw new BadRequestException(nameof(request.Name), $"There is already a stage with the name '{request.Name}'.");
            }

            var stage = new Stage
            {
                Name = request.Name,
                GameID = request.GameID,
            };

            dbContext.Add(stage);
            await dbContext.SaveChangesAsync();

            return stage;
        }
    }
}