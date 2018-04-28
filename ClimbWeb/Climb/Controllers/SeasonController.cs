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
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class SeasonController : Controller
    {
        private readonly ISeasonService seasonService;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<SeasonController> logger;

        public SeasonController(ISeasonService seasonService, ApplicationDbContext dbContext, ILogger<SeasonController> logger)
        {
            this.dbContext = dbContext;
            this.seasonService = seasonService;
            this.logger = logger;
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
                return this.CodeResultAndLog(HttpStatusCode.NotFound, $"No League with ID '{leagueID}' found.", logger);
            }

            return this.CodeResult(HttpStatusCode.OK, league.Seasons);
        }

        [HttpPost("/api/v1/seasons/create")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(Season))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string), "Start and end date issues.")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Can't find league.")]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            if(!await dbContext.Leagues.AnyAsync(l => l.ID == request.LeagueID))
            {
                return this.CodeResultAndLog(HttpStatusCode.NotFound, $"No League with ID '{request.LeagueID}' found.", logger);
            }

            if(request.StartDate < DateTime.Now)
            {
                return this.CodeResultAndLog(HttpStatusCode.BadRequest, "Can't have start date in the past.", logger);
            }

            if(request.EndDate < request.StartDate)
            {
                return this.CodeResultAndLog(HttpStatusCode.BadRequest, "Can't have an end date earlier than the start date.", logger);
            }

            var season = await seasonService.Create(request.LeagueID, request.StartDate, request.EndDate);

            return this.CodeResultAndLog(HttpStatusCode.Created, season, "Season created.", logger);
        }
    }
}