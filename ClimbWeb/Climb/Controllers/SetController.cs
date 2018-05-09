using System.Collections.Generic;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Requests.Sets;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class SetController : Controller
    {
        private readonly ISetService setService;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<SetController> logger;

        public SetController(ISetService setService, ApplicationDbContext dbContext, ILogger<SetController> logger)
        {
            this.setService = setService;
            this.dbContext = dbContext;
            this.logger = logger;
        }

        [HttpPost("/api/v1/sets/submit/{setID:int}")]
        public async Task<IActionResult> Submit(int setID, List<MatchForm> matches)
        {
            await Task.CompletedTask;

            return Ok();
        }
    }
}