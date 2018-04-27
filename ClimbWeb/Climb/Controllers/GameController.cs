using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Models;
using Climb.Requests.Games;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class GameController : BaseController<GameController>
    {
        private readonly IGameService gameService;
        private readonly ApplicationDbContext dbContext;

        public GameController(IGameService gameService, ApplicationDbContext dbContext, ILogger<GameController> logger)
            : base(logger)
        {
            this.gameService = gameService;
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
        [SwaggerResponse(HttpStatusCode.Created, typeof(Game))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string), "Game name is taken.")]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            if(await dbContext.Games.AnyAsync(g => g.Name == request.Name))
            {
                return CodeResult(HttpStatusCode.BadRequest, $"Game with name '{request.Name}' already exists.");
            }

            var game = await gameService.Create(request.Name);

            return CodeResultAndLog(HttpStatusCode.Created, game, "Game created.");
        }

        [HttpGet("/api/v1/games")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<Game>))]
        public async Task<IActionResult> ListAll()
        {
            var games = await dbContext.Games.ToListAsync();

            return CodeResult(HttpStatusCode.OK, games);
        }
    }
}