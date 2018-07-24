using System;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Requests.Games;
using Climb.Services;
using Climb.Services.ModelServices;
using Climb.ViewModels.Games;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class GameController : BaseController<GameController>
    {
        private readonly IGameService gameService;
        private readonly ICdnService cdnService;

        public GameController(IGameService gameService, ApplicationDbContext dbContext, ILogger<GameController> logger, UserManager<ApplicationUser> userManager, ICdnService cdnService)
            : base(logger, userManager, dbContext)
        {
            this.gameService = gameService;
            this.cdnService = cdnService;
        }

        [HttpGet("games")]
        public async Task<IActionResult> Index()
        {
            var user = await GetViewUserAsync();
            var games = await dbContext.Games.Include(g => g.Leagues).ThenInclude(l => l.Members).AsNoTracking().ToArrayAsync();

            var viewModel = new IndexViewModel(user, games);
            return View(viewModel);
        }

        [HttpGet("games/home/{gameID:int}")]
        public async Task<IActionResult> Home(int gameID)
        {
            var user = await GetViewUserAsync();
            var game = await dbContext.Games.Include(g => g.Characters).AsNoTracking().Include(g => g.Stages).AsNoTracking().FirstOrDefaultAsync(g => g.ID == gameID);
            if(game == null)
            {
                return NotFound();
            }

            var viewModel = new HomeViewModel(user, game);
            return View(viewModel);
        }

        [HttpGet("games/characters/add/{gameID:int}")]
        public async Task<IActionResult> CharacterAdd(int gameID)
        {
            var user = await GetViewUserAsync();
            var game = await dbContext.Games.FirstOrDefaultAsync(g => g.ID == gameID);
            if(game == null)
            {
                return NotFound();
            }

            var viewModel = new CharacterAddViewModel(user, game);
            return View(viewModel);
        }

        [HttpPost("games/characters/add")]
        public async Task<IActionResult> CharacterAddPost(AddCharacterRequest request)
        {
            try
            {
                var imageKey = await cdnService.UploadImageAsync(request.Image, ClimbImageRules.CharacterPic);
                await gameService.AddCharacter(request.GameID, request.Name, imageKey);
                return RedirectToAction("Home", new {request.GameID});
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpGet("games/stages/add/{gameID:int}")]
        public async Task<IActionResult> StageAdd(int gameID)
        {
            var user = await GetViewUserAsync();
            var game = await dbContext.Games.FirstOrDefaultAsync(g => g.ID == gameID);
            if(game == null)
            {
                return NotFound();
            }

            var viewModel = new StageAddViewModel(user, game);
            return View(viewModel);
        }

        [HttpPost("games/stages/add")]
        public async Task<IActionResult> StageAddPost(AddStageRequest request)
        {
            try
            {
                await gameService.AddStage(request);
                return RedirectToAction("Home", new {request.GameID});
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpPost("games/create")]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            try
            {
                var game = await gameService.Create(request);
                logger.LogInformation($"Game {game.ID} created");

                return RedirectToAction("Home", new
                {
                    gameID = game.ID
                });
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }
    }
}