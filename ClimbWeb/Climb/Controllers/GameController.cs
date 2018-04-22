using System.Net;
using System.Threading.Tasks;
using Climb.Models;
using Climb.Requests.Games;
using Climb.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Climb.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameRepository gameRepository;

        public GameController(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        [HttpPost("/api/v1/games/create")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(Game), IsNullable = false)]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string), IsNullable = false)]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            if(await gameRepository.AnyExist(request.Name))
            {
                return BadRequest($"Game with name '{request.Name}' already exists.");
            }

            var game = await gameRepository.Create(request.Name);

            return CreatedAtRoute($"games/{game.ID}", game);
        }
    }
}