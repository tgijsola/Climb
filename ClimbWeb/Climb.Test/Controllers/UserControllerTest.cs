using System.Net;
using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        private UserController testObj;
        private IApplicationUserService applicationUserService;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();
            applicationUserService = Substitute.For<IApplicationUserService>();
            var logger = Substitute.For<ILogger<UserController>>();

            testObj = new UserController(applicationUserService, logger);
        }

        [Test]
        public async Task UploadProfilePic_Valid_Created()
        {
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);
            var file = Substitute.For<IFormFile>();

            var result = await testObj.UploadProfilePic(user.Id, file);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Created);
        }
    }
}