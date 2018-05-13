using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Extensions;
using Climb.Requests.Sets;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost("/api/v1/sets/submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitRequest request)
        {
            try
            {
                await setService.Update(request.SetID, request.Matches);
            }
            catch(NotFoundException exception)
            {
                return this.CodeResultAndLog(HttpStatusCode.NotFound, exception.Message, logger);
            }
            catch(Exception exception)
            {
                logger.LogError(exception, "Service error updating set.");
                return this.CodeResult(HttpStatusCode.InternalServerError, "Could not submit set.");
            }

            return Ok();
        }
    }
}