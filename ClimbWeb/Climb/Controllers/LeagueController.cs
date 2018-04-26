using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Extensions;
using Climb.Models;
using Climb.Requests.Leagues;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;

namespace Climb.Controllers
{
    public class LeagueController : Controller
    {
        private readonly ILeagueService leagueService;
        private readonly ApplicationDbContext dbContext;

        public LeagueController(ILeagueService leagueService, ApplicationDbContext dbContext)
        {
            this.leagueService = leagueService;
            this.dbContext = dbContext;
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
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<League>), IsNullable = false)]
        public async Task<IActionResult> ListAll()
        {
            var leagues = await dbContext.Leagues.ToListAsync();

            return Ok(leagues);
        }

        [HttpPost("/api/v1/leagues/create")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(League), IsNullable = false)]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), IsNullable = false)]
        [SwaggerResponse(HttpStatusCode.Conflict, typeof(string), IsNullable = false)]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            if(!await dbContext.Games.AnyAsync(g => g.ID == request.GameID))
            {
                return NotFound($"No Game with ID '{request.GameID}' found.");
            }

            if(await dbContext.Leagues.AnyAsync(l => l.Name == request.Name))
            {
                return this.CodeResult(StatusCodes.Status409Conflict, $"League with name '{request.Name}' already exists.");
            }

            var league = await leagueService.Create(request.Name, request.GameID);

            return new ObjectResult(league) {StatusCode = StatusCodes.Status201Created};
        }
    }
}