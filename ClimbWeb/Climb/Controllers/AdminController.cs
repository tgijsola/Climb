using System;
using System.Threading.Tasks;
using Climb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Climb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.context = context;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost("admin/data/migrate")]
        public async Task<IActionResult> Migrate()
        {
            try
            {
                await DataMigrator.MigrateV1(context, userManager, configuration.GetConnectionString("ClimbV1"));
                return Ok();
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                return StatusCode(500, exception);
            }
        }
    }
}