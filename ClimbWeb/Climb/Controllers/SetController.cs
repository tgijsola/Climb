using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Requests.Sets;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class SetController : BaseController<SetController>
    {
        private readonly ISetService setService;

        public SetController(ISetService setService, ILogger<SetController> logger)
            : base(logger)
        {
            this.setService = setService;
        }

        [HttpPost("/api/v1/sets/submit")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string))]
        public async Task<IActionResult> Submit([FromBody] SubmitRequest request)
        {
            try
            {
                await setService.Update(request.SetID, request.Matches);
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }

            return Ok();
        }
    }
}