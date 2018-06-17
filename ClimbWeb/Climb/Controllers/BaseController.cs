using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;

namespace Climb.Controllers
{
    [SwaggerIgnore]
    public abstract class BaseController<T> : Controller where T : Controller
    {
        protected readonly ILogger<T> logger;
        protected readonly UserManager<ApplicationUser> userManager;
        protected readonly ApplicationDbContext dbContext;

        protected BaseController(ILogger<T> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        protected async Task<ApplicationUser> GetViewUserAsync()
        {
            var appUser = await userManager.GetUserAsync(User);
            if(appUser == null)
            {
                return null;
            }

            var loadedUser = await dbContext.Users
                .Include(u => u.LeagueUsers).ThenInclude(lu => lu.League).AsNoTracking()
                .Include(u => u.LeagueUsers).ThenInclude(lu => lu.Seasons).ThenInclude(slu => slu.Season).AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == appUser.Id);

            return loadedUser;
        }

        protected IActionResult GetExceptionResult(Exception exception, object request)
        {
            switch(exception)
            {
                case NotFoundException _: return CodeResultAndLog(HttpStatusCode.NotFound, exception.Message);
                case BadRequestException _: return CodeResultAndLog(HttpStatusCode.BadRequest, exception.Message);
                case ConflictException _: return CodeResultAndLog(HttpStatusCode.Conflict, exception.Message);
                default:
                    logger.LogError(exception, $"Error handling request\n{request}");
                    return CodeResult(HttpStatusCode.InternalServerError, "Server Error");
            }
        }

        protected ObjectResult CodeResult(HttpStatusCode code, object value)
        {
            return new ObjectResult(value) {StatusCode = (int)code};
        }

        protected ObjectResult CodeResultAndLog(HttpStatusCode code, object value, string message)
        {
            logger.LogInformation(message);
            return new ObjectResult(value) {StatusCode = (int)code};
        }

        protected ObjectResult CodeResultAndLog(HttpStatusCode code, string value)
        {
            logger.LogInformation(value);
            return new ObjectResult(value) {StatusCode = (int)code};
        }
    }
}