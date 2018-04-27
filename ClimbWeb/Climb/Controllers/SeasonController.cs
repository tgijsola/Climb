using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Extensions;
using Climb.Models;
using Climb.Requests.Seasons;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Climb.Controllers
{
    public class SeasonController : Controller
    {
        private readonly ISeasonService seasonService;
        private readonly ApplicationDbContext dbContext;

        public SeasonController(ISeasonService seasonService, ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.seasonService = seasonService;
        }

        [HttpGet("/seasons/{*page}")]
        [SwaggerIgnore]
        public IActionResult Index()
        {
            ViewData["Title"] = "Season";
            ViewData["Script"] = "seasons";
            return View("~/Views/Page.cshtml");
        }

        [HttpGet("/api/v1/seasons")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Season[]))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Can't find league.")]
        public async Task<IActionResult> ListForLeague(int leagueID)
        {
            var league = await dbContext.Leagues
                .Include(l => l.Seasons).AsNoTracking()
                .FirstOrDefaultAsync(l => l.ID == leagueID);
            if(league == null)
            {
                return NotFound($"No League with ID '{leagueID}' found.");
            }

            return Ok(league.Seasons);
        }

        [HttpPost("/api/v1/seasons/create")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(Season))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string), "Start and end date issues.")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Can't find league.")]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            if(!await dbContext.Leagues.AnyAsync(l => l.ID == request.LeagueID))
            {
                return NotFound($"No League with ID '{request.LeagueID}' found.");
            }

            if(request.StartDate < DateTime.Now)
            {
                return BadRequest("Can't have start date in the past.");
            }

            if(request.EndDate < request.StartDate)
            {
                return BadRequest("Can't have an end date earlier than the start date.");
            }

            var season = await seasonService.Create(request.LeagueID, request.StartDate, request.EndDate);

            return this.CodeResult(HttpStatusCode.Created, season);
        }
    }
}