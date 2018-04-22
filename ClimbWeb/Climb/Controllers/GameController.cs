using System.Collections.Generic;
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
        
        [HttpGet("/games/{*page}")]
        [SwaggerIgnore]
        public IActionResult Index()
        {
            ViewData["Title"] = "Game";
            ViewData["Script"] = "games";
            return View("~/Views/Page.cshtml");
        }

        [HttpPost("/api/v1/games/create")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Game), IsNullable = false)]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string), IsNullable = false)]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            if(await gameRepository.AnyExist(request.Name))
            {
                return BadRequest($"Game with name '{request.Name}' already exists.");
            }

            var game = await gameRepository.Create(request.Name);

            return Ok(game);
        }

        [HttpGet("/api/v1/games")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<Game>), IsNullable = false)]
        public async Task<IActionResult> ListAll()
        {
            var games = await gameRepository.ListAll();

            return Ok(games);
        }
    }
}