using Microsoft.AspNetCore.Mvc;

namespace Climb.Extensions
{
    public static class ControllerExtensions
    {
        public static ObjectResult CodeResult(this Controller controller, int code, object value)
        {
            return new ObjectResult(value){StatusCode = code};
        }
    }
}