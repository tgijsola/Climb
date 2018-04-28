using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Climb.Extensions
{
    public static class ControllerExtensions
    {
        public static ObjectResult CodeResult(this Controller controller, HttpStatusCode code, object value)
        {
            return new ObjectResult(value) {StatusCode = (int)code};
        }

        public static ObjectResult CodeResultAndLog(this Controller controller,HttpStatusCode code, object value, string message, ILogger logger)
        {
            logger.LogInformation(message);
            return new ObjectResult(value) {StatusCode = (int)code};
        }

        public static ObjectResult CodeResultAndLog(this Controller controller,HttpStatusCode code, string value, ILogger logger)
        {
            logger.LogInformation(value);
            return new ObjectResult(value) {StatusCode = (int)code};
        }
    }
}