using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using Climb.Requests.Games;
using Climb.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;

namespace Climb.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameRepository gameRepository;
        private readonly ApplicationDbContext dbContext;

        public GameController(IGameRepository gameRepository, ApplicationDbContext dbContext)
        {
            this.gameRepository = gameRepository;
            this.dbContext = dbContext;
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
        [SwaggerResponse(HttpStatusCode.Created, typeof(Game), IsNullable = false)]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string), IsNullable = false)]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            if(await dbContext.Games.AnyAsync(g => g.Name == request.Name))
            {
                return BadRequest($"Game with name '{request.Name}' already exists.");
            }

            var game = await gameRepository.Create(request.Name);

            return new ObjectResult(game) {StatusCode = StatusCodes.Status201Created};
        }

        [HttpGet("/api/v1/games")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<Game>), IsNullable = false)]
        public async Task<IActionResult> ListAll()
        {
            var games = await dbContext.Games.ToListAsync();

            return Ok(games);
        }
    }
}