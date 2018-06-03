using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Requests.Sets;
using Climb.Responses.Sets;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        [HttpPost("/api/v1/sets/submit")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(SetDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string))]
        public async Task<IActionResult> Submit([FromBody] SubmitRequest request)
        {
            try
            {
                var set = await setService.Update(request.SetID, request.Matches);
                dbContext.Entry(set).Reference(s => s.League).Load();
                var response = SetDto.Create(set, set.League.GameID);
                return CodeResultAndLog(HttpStatusCode.OK, response, $"Set {set.ID} updated.");
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpGet("/api/v1/sets/{setID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(SetDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        public async Task<IActionResult> Get(int setID)
        {
            var set = await dbContext.Sets
                .Include(s => s.Matches).ThenInclude(m => m.MatchCharacters).AsNoTracking()
                .Include(s => s.League).AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == setID);
            if(set == null)
            {
                return CodeResultAndLog(HttpStatusCode.NotFound, $"Could not find Set with ID '{setID}'.");
            }

            var dto = SetDto.Create(set, set.League.GameID);

            return CodeResult(HttpStatusCode.OK, dto);
        }
    }
}