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
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;
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


        private const string Email = "a@a.com";
        private const string Password = "Abc_123";

        private TestController testObj;
        private FakeUserManager userManager;
        private FakeSignInManager signInManager;
        private ILogger<AccountController> logger;
        private IEmailSender emailSender;
        private IConfiguration configuration;
        private ITokenHelper tokenHelper;
        private IUrlUtility urlUtility;
        private IObjectModelValidator objectModelValidator;

        [SetUp]
        public void SetUp()
        {
            userManager = Substitute.For<FakeUserManager>();
            signInManager = Substitute.For<FakeSignInManager>();
            logger = Substitute.For<ILogger<AccountController>>();
            emailSender = Substitute.For<IEmailSender>();
            configuration = Substitute.For<IConfiguration>();
            tokenHelper = Substitute.For<ITokenHelper>();
            urlUtility = Substitute.For<IUrlUtility>();
            objectModelValidator = Substitute.For<IObjectModelValidator>();

            testObj = new TestController(signInManager, logger, userManager, emailSender, configuration, tokenHelper, urlUtility)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()},
                ObjectValidator = objectModelValidator,
            };
        }

        [Test]
        public async Task Register_Valid_Ok()
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
    }
}