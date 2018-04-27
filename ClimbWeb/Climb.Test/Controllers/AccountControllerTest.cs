using System.Net;
using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Requests.Account;
using Climb.Services;
using Climb.Test.Fakes;
using Climb.Test.Utilities;
using Climb.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class AccountControllerTest
    {
        private class TestController : AccountController
        {
            public bool willValidateTrue = true;

            public TestController(SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration, ITokenHelper tokenHelper, IUrlUtility urlUtility)
                : base(signInManager, logger, userManager, emailSender, configuration, tokenHelper, urlUtility)
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
        private ILogger<AccountController> logger;
        private IEmailSender emailSender;
        private IConfiguration configuration;
        private ITokenHelper tokenHelper;
        private IUrlUtility urlUtility;

        [SetUp]
        public void SetUp()
        {
            userManager = Substitute.For<FakeUserManager>();
            signInManager = Substitute.For<FakeSignInManager>();
            logger = Substitute.For<ILogger<AccountController>>();
            emailSender = Substitute.For<IEmailSender>();
            configuration = Substitute.For<IConfiguration>();
            configuration["SecurityKey"].Returns("key");
            tokenHelper = Substitute.For<ITokenHelper>();
            urlUtility = Substitute.For<IUrlUtility>();

            testObj = new TestController(signInManager, logger, userManager, emailSender, configuration, tokenHelper, urlUtility)
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