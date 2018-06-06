using System.Net;
using System.Threading.Tasks;
using Climb.API;
using Climb.Data;
using Climb.Requests.Account;
using Climb.Services;
using Climb.Services.ModelServices;
using Climb.Test.Fakes;
using Climb.Test.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class AccountApiTest
    {
        private class TestController : AccountApi
        {
            public bool willValidateTrue = true;

            public TestController(SignInManager<ApplicationUser> signInManager, ITokenHelper tokenHelper, ILogger<AccountApi> logger, IApplicationUserService applicationUserService)
                : base(signInManager, tokenHelper, logger, applicationUserService)
            {
            }

            public override bool TryValidateModel(object model)
            {
                return willValidateTrue;
            }
        }

        private TestController testObj;
        private FakeUserManager userManager;
        private FakeSignInManager signInManager;

        [SetUp]
        public void SetUp()
        {
            userManager = Substitute.For<FakeUserManager>();
            signInManager = Substitute.For<FakeSignInManager>();
            var logger = Substitute.For<ILogger<AccountApi>>();
            var tokenHelper = Substitute.For<ITokenHelper>();
            var applicationUserService = Substitute.For<IApplicationUserService>();

            testObj = new TestController(signInManager, tokenHelper, logger, applicationUserService)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()},
            };
        }

        // TODO: LogIn

        [Test]
        public async Task Register_Valid_Created()
        {
            userManager.CreateAsync(null, null).ReturnsForAnyArgs(IdentityResult.Success);

            var request = new RegisterRequest();

            var result = await testObj.Register(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Created);
        }

        [Test]
        public async Task Register_NotEmail_BadRequest()
        {
            userManager.CreateAsync(null, null).ReturnsForAnyArgs(IdentityResult.Success);
            testObj.willValidateTrue = false;

            var request = new RegisterRequest();

            var result = await testObj.Register(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task LogIn_Valid_Ok()
        {
            signInManager.PasswordSignInAsync("", "", false, false).ReturnsForAnyArgs(SignInResult.Success);

            var request = new LoginRequest();

            var result = await testObj.LogIn(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.OK);
        }

        [Test]
        public async Task LogIn_IncorrectCreds_BadRequest()
        {
            signInManager.PasswordSignInAsync("", "", false, false).ReturnsForAnyArgs(SignInResult.Failed);

            var request = new LoginRequest();

            var result = await testObj.LogIn(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.BadRequest);
        }
    }
}