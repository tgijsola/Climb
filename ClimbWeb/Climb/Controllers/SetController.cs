using Climb.Attributes;
using Climb.Data;
using Climb.Models;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Climb.Controllers
{
    public class SetController : BaseController<SetController>
    {
        private readonly ISetService setService;

        public SetController(ApplicationDbContext dbContext, ISetService setService, ILogger<SetController> logger, UserManager<ApplicationUser> userManager)
            : base(logger, userManager, dbContext)
        {
            this.setService = setService;
        }

        [HttpPost("api/v1/sets/challenge")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(SetRequest))]
        public async Task<IActionResult> ChallengeUser(int requesterID, int challengedID)
        {
            try
            {
                var request = await setService.RequestSetAsync(requesterID, challengedID);
                return CodeResultAndLog(HttpStatusCode.Created, request, $"Member {requesterID} challenged {challengedID}.");
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, new { requesterID, challengedID });
            }
        }
    }
}