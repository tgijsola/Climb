using System;
using System.Threading.Tasks;
using Climb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Climb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost("admin/data/migrate")]
        public async Task<IActionResult> Migrate()
        {
            try
            {
                await DataMigrator.MigrateV1(context, userManager);
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