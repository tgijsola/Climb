using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Climb.Extensions
{
    public static class ControllerExtensions
    {
        public static ObjectResult CodeResult(this Controller controller, HttpStatusCode code, object value)
        {
            return new ObjectResult(value){StatusCode = (int)code};
        }
    }
}