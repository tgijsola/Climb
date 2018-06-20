using System.Threading.Tasks;
using Climb.Data;
using Climb.ViewModels;
using Climb.ViewModels.Site;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Climb.Controllers
{
    public class SiteController : BaseController<SiteController>
    {
        public SiteController(ILogger<SiteController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
            : base(logger, userManager, dbContext)
        {
        }

        [HttpGet("")]
        public async Task<IActionResult> Home()
        {
            var user = await GetViewUserAsync();

            var viewModel = await HomeViewModel.Create(user, dbContext);
            return View(viewModel);
        }

        public IActionResult Error(int? statusCode = null)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            ViewData["ErrorUrl"] = feature?.OriginalPath;
            ViewData["ErrorQuerystring"] = feature?.OriginalQueryString;

            if (statusCode.HasValue)
            {
                if (statusCode == 404 || statusCode == 500)
                {
                    var viewName = statusCode.ToString();
                    return View($"Error{viewName}");
                }
            }

            return View("Error500");
        }

        [HttpGet("Support")]
        public async Task<IActionResult> Support()
        {
            var user = await GetViewUserAsync();
            
            ViewData["Success"] = TempData.ContainsKey("success");

            var viewModel = new BaseViewModel(user);
            return View(viewModel);
        }

        [HttpPost("SendSupportTicket")]
        public IActionResult SendSupportTicket(string summary, string description)
        {
            TempData["success"] = "here";

            return RedirectToAction("Support");
        }
    }
}