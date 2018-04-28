using System.Net;
using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Requests.Account;
using Climb.Services;
using Climb.Test.Fakes;
using Climb.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using ControllerUtility = Climb.Test.Utilities.ControllerUtility;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class AccountControllerTest
    {
        private class TestController : AccountController
        {
            public bool willValidateTrue = true;

            public TestController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration, ITokenHelper tokenHelper, IUrlUtility urlUtility, ILogger<AccountController> logger)
                : base(signInManager, userManager, emailSender, configuration, tokenHelper, urlUtility, logger)
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
            var logger = Substitute.For<ILogger<AccountController>>();
            var emailSender = Substitute.For<IEmailSender>();
            var configuration = Substitute.For<IConfiguration>();
            configuration["SecurityKey"].Returns("key");
            var tokenHelper = Substitute.For<ITokenHelper>();
            var urlUtility = Substitute.For<IUrlUtility>();

            testObj = new TestController(signInManager, userManager, emailSender, configuration, tokenHelper, urlUtility, logger)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()},
            };
        }

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