using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Extensions;
using Climb.Requests.Sets;
using Climb.Responses.Sets;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class SetController : BaseController<SetController>
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ISetService setService;

        public SetController(ApplicationDbContext dbContext, ISetService setService, ILogger<SetController> logger)
            : base(logger)
        {
            this.setService = setService;
            this.dbContext = dbContext;
        }

        [HttpGet("/sets/{*page}")]
        [SwaggerIgnore]
        public IActionResult Index()
        {
            ViewData["Title"] = "Set";
            ViewData["Script"] = "sets";
            return View("~/Views/Page.cshtml");
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

        [HttpGet("/api/v1/sets/{setID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(SetGetResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        public async Task<IActionResult> Get(int setID)
        {
            var set = await dbContext.Sets.FirstOrDefaultAsync(s => s.ID == setID);
            if(set == null)
            {
                return this.CodeResultAndLog(HttpStatusCode.NotFound, $"Could not find Set with ID '{setID}'.", logger);
            }

            return this.CodeResult(HttpStatusCode.OK, set);
        }
    }
}