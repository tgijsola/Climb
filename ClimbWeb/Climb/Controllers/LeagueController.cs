using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Models;
using Climb.Requests.Leagues;
using Climb.Responses.Models;
using Climb.Services.ModelServices;
using Climb.ViewModels.Leagues;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class LeagueController : BaseController<LeagueController>
    {
        private readonly ILeagueService leagueService;

        public LeagueController(ILeagueService leagueService, ApplicationDbContext dbContext, ILogger<LeagueController> logger, UserManager<ApplicationUser> userManager)
            : base(logger, userManager, dbContext)
        {
            this.leagueService = leagueService;
        }

        [HttpGet("leagues")]
        public async Task<IActionResult> Index()
        {
            var user = await GetViewUserAsync();
            var leagues = await dbContext.Leagues
                .Include(l => l.Members).AsNoTracking()
                .Include(l => l.Game).AsNoTracking()
                .ToArrayAsync();
            var games = await dbContext.Games.ToArrayAsync();

            var viewModel = new IndexViewModel(user, leagues, games);
            return View(viewModel);
        }

        [HttpGet("leagues/home/{leagueID:int}")]
        public async Task<IActionResult> Home(int leagueID)
        {
            var user = await GetViewUserAsync();
            var league = await dbContext.Leagues.FirstOrDefaultAsync(l => l.ID == leagueID);

            var viewModel = new HomeViewModel(user, league);

            return View(viewModel);
        }

        [HttpPost("leagues/create")]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            try
            {
                var league = await leagueService.Create(request.Name, request.GameID);
                logger.LogInformation($"League {league.ID} created.");
                
                return RedirectToAction("Home", new {leagueID = league.ID});
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpGet("/api/v1/leagues")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<League>))]
        public async Task<IActionResult> ListAll()
        {
            var leagues = await dbContext.Leagues.ToListAsync();

            return CodeResult(HttpStatusCode.OK, leagues);
        }

        [HttpGet("/api/v1/leagues/{leagueID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(League))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        public async Task<IActionResult> Get(int leagueID)
        {
            var league = await dbContext.Leagues.FirstOrDefaultAsync(l => l.ID == leagueID);
            if(league == null)
            {
                return CodeResultAndLog(HttpStatusCode.NotFound, $"No League with ID '{leagueID}' found.");
            }

            return CodeResult(HttpStatusCode.OK, league);
        }

        [HttpPost("/api/v1/leagues/create")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(League))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Can't find game.")]
        [SwaggerResponse(HttpStatusCode.Conflict, typeof(string), "League name taken.")]
        public async Task<IActionResult> Create_API(CreateRequest request)
        {
            try
            {
                var league = await leagueService.Create(request.Name, request.GameID);
                return CodeResult(HttpStatusCode.Created, league);
            }
            catch(Exception exception)
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
            try
            {
                var leagueUser = await leagueService.Join(request.LeagueID, request.UserID);
                return CodeResultAndLog(HttpStatusCode.Created, leagueUser, "User joined league.");
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpGet("/api/v1/leagues/user/{userID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(LeagueUserDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        public async Task<IActionResult> GetUser(int userID)
        {
            var leagueUser = await dbContext.LeagueUsers
                .Include(lu => lu.User).AsNoTracking()
                .FirstOrDefaultAsync(lu => lu.ID == userID);
            if(leagueUser == null)
            {
                return CodeResultAndLog(HttpStatusCode.NotFound, $"Could not find League User with ID '{userID}'.");
            }

            var response = new LeagueUserDto(leagueUser);
            return CodeResult(HttpStatusCode.OK, response);
        }

        [HttpGet("/api/v1/leagues/seasons/{leagueID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Season[]))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Can't find league.")]
        public async Task<IActionResult> GetSeasons(int leagueID)
        {
            var league = await dbContext.Leagues
                .Include(l => l.Seasons).AsNoTracking()
                .FirstOrDefaultAsync(l => l.ID == leagueID);
            if(league == null)
            {
                return CodeResultAndLog(HttpStatusCode.NotFound, $"No League with ID '{leagueID}' found.");
            }

            return CodeResult(HttpStatusCode.OK, league.Seasons);
        }
    }
}