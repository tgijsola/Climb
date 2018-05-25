using System;
using System.Linq;
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
    public class SeasonController : BaseController<SeasonController>
    {
        private readonly ISeasonService seasonService;
        private readonly ApplicationDbContext dbContext;

        public SeasonController(ISeasonService seasonService, ApplicationDbContext dbContext, ILogger<SeasonController> logger)
            : base(logger)
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

        [HttpGet("/api/v1/seasons/{seasonID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Season))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        public async Task<IActionResult> Get(int seasonID)
        {
            var season = await dbContext.Seasons
                .Include(s => s.Participants).ThenInclude(slu => slu.LeagueUser).AsNoTracking()
                .Include(s => s.Sets).AsNoTracking()
                .Include(s => s.League).AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == seasonID);
            if(season == null)
            {
                return this.CodeResultAndLog(HttpStatusCode.NotFound, $"No Season with ID '{seasonID}' found.", logger);
            }

            return this.CodeResult(HttpStatusCode.OK, season);
        }

        [HttpGet("/api/v1/seasons/sets/{seasonID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Set[]))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        public async Task<IActionResult> Sets(int seasonID)
        {
            var season = await dbContext.Seasons
                .Include(s => s.Sets).AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == seasonID);
            if(season == null)
            {
                return this.CodeResultAndLog(HttpStatusCode.NotFound, $"No Season with ID '{seasonID}' found.", logger);
            }

            return this.CodeResult(HttpStatusCode.OK, season.Sets);
        }

        [HttpGet("/api/v1/seasons/participants/{seasonID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(LeagueUser[]))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        public async Task<IActionResult> Participants(int seasonID)
        {
            var season = await dbContext.Seasons
                .Include(s => s.Participants).ThenInclude(slu => slu.LeagueUser).AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == seasonID);
            if(season == null)
            {
                return this.CodeResultAndLog(HttpStatusCode.NotFound, $"No Season with ID '{seasonID}' found.", logger);
            }

            var participants = season.Participants.Select(slu => slu.LeagueUser);
            return this.CodeResult(HttpStatusCode.OK, participants);
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
            if(request.StartDate < DateTime.Now)
            {
                return this.CodeResultAndLog(HttpStatusCode.BadRequest, "Can't have start date in the past.", logger);
            }

            if(request.EndDate < request.StartDate)
            {
                return this.CodeResultAndLog(HttpStatusCode.BadRequest, "Can't have an end date earlier than the start date.", logger);
            }

            try
            {
                var season = await seasonService.Create(request.LeagueID, request.StartDate, request.EndDate);
                return this.CodeResultAndLog(HttpStatusCode.Created, season, "Season created.", logger);
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpPost("/api/v1/seasons/start")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(Set[]))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(string), "Error trying to create sets for the season schedule.")]
        public async Task<IActionResult> Start(int seasonID)
        {
            if(!await dbContext.Seasons.AnyAsync(s => s.ID == seasonID))
            {
                return this.CodeResultAndLog(HttpStatusCode.NotFound, $"No Season with ID '{seasonID}' found.", logger);
            }

            try
            {
                var sets = await seasonService.GenerateSchedule(seasonID);
                return this.CodeResultAndLog(HttpStatusCode.Created, sets, "Schedule created.", logger);
            }
            catch(Exception exception)
            {
                logger.LogError(exception, $"Could not create schedule for Season {seasonID}.");
                return this.CodeResult(HttpStatusCode.InternalServerError, $"Could not create schedule for Season '{seasonID}'");
            }
        }
    }
}