using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Extensions;
using Climb.Models;
using Climb.Requests.Leagues;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class LeagueController : Controller
    {
        private readonly ILeagueService leagueService;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<LeagueController> logger;

        public LeagueController(ILeagueService leagueService, ApplicationDbContext dbContext, ILogger<LeagueController> logger)
        {
            this.leagueService = leagueService;
            this.dbContext = dbContext;
            this.logger = logger;
        }

        [HttpGet("/leagues/{*page}")]
        [SwaggerIgnore]
        public IActionResult Index()
        {
            ViewData["Title"] = "League";
            ViewData["Script"] = "leagues";
            return View("~/Views/Page.cshtml");
        }

        [HttpGet("/api/v1/leagues")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<League>))]
        public async Task<IActionResult> ListAll()
        {
            var leagues = await dbContext.Leagues.ToListAsync();

            return this.CodeResult(HttpStatusCode.OK, leagues);
        }

        [HttpPost("/api/v1/leagues/create")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(League))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Can't find game.")]
        [SwaggerResponse(HttpStatusCode.Conflict, typeof(string), "League name taken.")]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            if(!await dbContext.Games.AnyAsync(g => g.ID == request.GameID))
            {
                return this.CodeResultAndLog(HttpStatusCode.NotFound, $"No Game with ID '{request.GameID}' found.", logger);
            }

            if(await dbContext.Leagues.AnyAsync(l => l.Name == request.Name))
            {
                return this.CodeResultAndLog(HttpStatusCode.Conflict, $"League with name '{request.Name}' already exists.", logger);
            }

            var league = await leagueService.Create(request.Name, request.GameID);

            return this.CodeResult(HttpStatusCode.Created, league);
        }
    }
}