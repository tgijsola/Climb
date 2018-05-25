using System;
using System.Net;
using Climb.Exceptions;
using Climb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public abstract class BaseController<T> : Controller where T : Controller
    {
        protected readonly ILogger<T> logger;

        protected BaseController(ILogger<T> logger)
        {
            this.logger = logger;
        }

        protected IActionResult GetExceptionResult(Exception exception, object request)
        {
            switch(exception)
            {
                case NotFoundException _: return this.CodeResultAndLog(HttpStatusCode.NotFound, exception.Message, logger);
                case BadRequestException _: return this.CodeResultAndLog(HttpStatusCode.BadRequest, exception.Message, logger);
                case ConflictException _: return this.CodeResultAndLog(HttpStatusCode.Conflict, exception.Message, logger);
                default:
                    logger.LogError(exception, $"Error handling request\n{request}");
                    return this.CodeResult(HttpStatusCode.InternalServerError, "Server Error");
            }
        }
    }
}