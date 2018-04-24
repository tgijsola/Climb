using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Climb.Extensions;
using Climb.Models;
using Climb.Requests.Leagues;
using Climb.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Climb.Controllers
{
    public class LeagueController : Controller
    {
        private readonly ILeagueRepository leagueRepository;
        private readonly IGameRepository gameRepository;

        public LeagueController(ILeagueRepository leagueRepository, IGameRepository gameRepository)
        {
            this.leagueRepository = leagueRepository;
            this.gameRepository = gameRepository;
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
            var leagues = await leagueRepository.ListAll();

            return Ok(leagues);
        }

        [HttpPost("/api/v1/leagues/create")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(League), IsNullable = false)]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), IsNullable = false)]
        [SwaggerResponse(HttpStatusCode.Conflict, typeof(string), IsNullable = false)]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            if(!await gameRepository.Any(g => g.ID == request.GameID))
            {
                return NotFound($"No Game with ID '{request.GameID}' found.");
            }

            if(await leagueRepository.Any(l => l.Name == request.Name))
            {
                return this.CodeResult(StatusCodes.Status409Conflict, $"League with name '{request.Name}' already exists.");
            }

            var league = await leagueRepository.Create(request.Name, request.GameID);

            return new ObjectResult(league) {StatusCode = StatusCodes.Status201Created};
        }
    }
}