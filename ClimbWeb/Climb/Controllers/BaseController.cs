using System.Net;
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

        public ObjectResult CodeResult(HttpStatusCode code, object value)
        {
            return new ObjectResult(value) {StatusCode = (int)code};
        }

        public ObjectResult CodeResultAndLog(HttpStatusCode code, object value, string message)
        {
            logger.LogInformation(message);
            return new ObjectResult(value) {StatusCode = (int)code};
        }

        public ObjectResult CodeResultAndLog(HttpStatusCode code, string value)
        {
            logger.LogInformation(value);
            return new ObjectResult(value) {StatusCode = (int)code};
        }
    }
}