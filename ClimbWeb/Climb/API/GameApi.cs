using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climb.API
{
    public class GameApi : BaseApi<GameApi>
    {
        private readonly ApplicationDbContext dbContext;

        public GameApi(ILogger<GameApi> logger, ApplicationDbContext dbContext)
            : base(logger)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("/api/v1/games/{gameID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Game))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        public async Task<IActionResult> Get(int gameID)
        {
            var game = await dbContext.Games
                .Include(g => g.Characters).AsNoTracking()
                .Include(g => g.Stages).AsNoTracking()
                .FirstOrDefaultAsync(g => g.ID == gameID);
            if(game == null)
            {
                return CodeResultAndLog(HttpStatusCode.NotFound, $"Could not find Game with ID '{gameID}'.");
            }

            return CodeResult(HttpStatusCode.OK, game);
        }

        [HttpGet("/api/v1/games")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<Game>))]
        public async Task<IActionResult> ListAll()
        {
            var games = await dbContext.Games.ToListAsync();

            return CodeResult(HttpStatusCode.OK, games);
        }
    }
}