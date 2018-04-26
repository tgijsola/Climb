using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Extensions;
using Climb.Models;
using Climb.Requests.Seasons;
using Climb.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;

namespace Climb.Controllers
{
    public class SeasonController : Controller
    {
        private readonly ISeasonRepository seasonRepository;
        private readonly ILeagueRepository leagueRepository;
        private readonly ApplicationDbContext dbContext;

        public SeasonController(ISeasonRepository seasonRepository, ILeagueRepository leagueRepository, ApplicationDbContext dbContext)
        {
            this.leagueRepository = leagueRepository;
            this.dbContext = dbContext;
            this.seasonRepository = seasonRepository;
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
        [SwaggerResponse(HttpStatusCode.OK, typeof(Season[]), IsNullable = false)]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), IsNullable = false, Description = "Can't find league.")]
        public async Task<IActionResult> ListForLeague(int leagueID)
        {
            var league = await dbContext.Leagues
                .Include(l => l.Seasons).AsNoTracking()
                .FirstAsync(l => l.ID == leagueID);
            if(league == null)
            {
                return NotFound($"No League with ID '{leagueID}' found.");
            }

            return Ok(league.Seasons);
        }

        [HttpPost("/api/v1/seasons/create")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(Season), IsNullable = false)]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), IsNullable = false, Description = "Can't find league.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string), IsNullable = false, Description = "Start and end date issues.")]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            if(!await leagueRepository.Any(l => l.ID == request.LeagueID))
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

            var season = await seasonRepository.Create(request.LeagueID, request.StartDate, request.EndDate);

            return this.CodeResult(StatusCodes.Status201Created, season);
        }
    }
}