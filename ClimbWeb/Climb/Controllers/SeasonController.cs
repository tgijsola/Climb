using Climb.Data;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class SeasonController : BaseController<SeasonController>
    {
        private readonly ISeasonService seasonService;

        public SeasonController(ISeasonService seasonService, ApplicationDbContext dbContext, ILogger<SeasonController> logger, UserManager<ApplicationUser> userManager)
            : base(logger, userManager, dbContext)
        {
            this.seasonService = seasonService;
        }
    }
}