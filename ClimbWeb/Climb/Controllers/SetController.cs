using Climb.Data;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("sets/fight")]
        public IActionResult Fight()
        {
            return View();
        }
    }
}