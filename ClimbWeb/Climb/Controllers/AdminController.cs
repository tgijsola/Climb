using System;
using System.Threading.Tasks;
using Climb.Data;
using Microsoft.AspNetCore.Mvc;

namespace Climb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;

        public AdminController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost("admin/data/migrate")]
        public async Task<IActionResult> Migrate()
        {
            try
            {
                await DataMigrator.MigrateV1(context);
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