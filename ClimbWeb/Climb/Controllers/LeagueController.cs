using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Extensions;
using Climb.Models;
using Climb.Requests.Leagues;
using Climb.Responses.Models;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class LeagueController : BaseController<LeagueController>
    {
        private readonly ILeagueService leagueService;
        private readonly ApplicationDbContext dbContext;

        public LeagueController(ILeagueService leagueService, ApplicationDbContext dbContext, ILogger<LeagueController> logger)
            : base(logger)
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
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<League>))]
        public async Task<IActionResult> ListAll()
        {
            var leagues = await dbContext.Leagues.ToListAsync();

            return this.CodeResult(HttpStatusCode.OK, leagues);
        }

        [HttpGet("/api/v1/leagues/{leagueID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(League))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        public async Task<IActionResult> Get(int leagueID)
        {
            var league = await dbContext.Leagues.FirstOrDefaultAsync(l => l.ID == leagueID);
            if(league == null)
            {
                return this.CodeResultAndLog(HttpStatusCode.NotFound, $"No League with ID '{leagueID}' found.", logger);
            }

            return this.CodeResult(HttpStatusCode.OK, league);
        }

        [HttpPost("/api/v1/leagues/create")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(League))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Can't find game.")]
        [SwaggerResponse(HttpStatusCode.Conflict, typeof(string), "League name taken.")]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            try
            {
                var league = await leagueService.Create(request.Name, request.GameID);
                return this.CodeResult(HttpStatusCode.Created, league);
            }
            catch (Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpPost("/api/v1/leagues/join")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(LeagueUser))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Can't find league.")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Can't find user.")]
        public async Task<IActionResult> Join(JoinRequest request)
        {
            if(!await dbContext.Users.AnyAsync(u => u.Id == request.UserID))
            {
                return this.CodeResultAndLog(HttpStatusCode.BadRequest, $"No User with ID '{request.UserID}' found.", logger);
            }

            try
            {
                var leagueUser = await leagueService.Join(request.LeagueID, request.UserID);
                return this.CodeResultAndLog(HttpStatusCode.Created, leagueUser, "User joined league.", logger);
            }
            catch (Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpGet("/api/v1/leagues/user/{userID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(LeagueUserDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        public async Task<IActionResult> GetUser(int userID)
        {
            var leagueUser = await dbContext.LeagueUsers.Include(lu => lu.User).AsNoTracking().FirstOrDefaultAsync(lu => lu.ID == userID);
            if(leagueUser == null)
            {
                return this.CodeResultAndLog(HttpStatusCode.NotFound, $"Could not find League User with ID '{userID}'.", logger);
            }

            var response = new LeagueUserDto(leagueUser);
            return this.CodeResult(HttpStatusCode.OK, response);
        }

        [HttpGet("/api/v1/leagues/seasons/{leagueID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Season[]))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Can't find league.")]
        public async Task<IActionResult> GetSeasons(int leagueID)
        {
            var league = await dbContext.Leagues.Include(l => l.Seasons).AsNoTracking().FirstOrDefaultAsync(l => l.ID == leagueID);
            if(league == null)
            {
                return this.CodeResultAndLog(HttpStatusCode.NotFound, $"No League with ID '{leagueID}' found.", logger);
            }

            return this.CodeResult(HttpStatusCode.OK, league.Seasons);
        }
    }
}