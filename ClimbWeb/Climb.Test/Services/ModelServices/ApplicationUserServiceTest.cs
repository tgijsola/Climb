using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Requests.Account;
using Climb.Services;
using Climb.Services.ModelServices;
using Climb.Test.Fakes;
using Climb.Test.Utilities;
using Climb.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Services.ModelServices
{
    // TODO: Register
    [TestFixture]
    public class ApplicationUserServiceTest
    {
        private const string ImageKey = "KEY";
        private const string ImageUrl = "IMAGEURL";
        private ApplicationUserService testObj;
        private ApplicationDbContext dbContext;
        private ICdnService cdnService;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();
            cdnService = Substitute.For<ICdnService>();

            var configuration = Substitute.For<IConfiguration>();
            configuration["SecurityKey"].Returns("key");

            var emailSender = Substitute.For<IEmailSender>();
            var tokenHelper = Substitute.For<ITokenHelper>();
            var urlUtility = Substitute.For<IUrlUtility>();
            var signInManager = new FakeSignInManager();
            var userManager = new FakeUserManager();

            testObj = new ApplicationUserService(dbContext, cdnService, signInManager, emailSender, configuration, tokenHelper, urlUtility, userManager);
        }

        [Test]
        public void LogIn_NoUser_NotFoundException()
        {
            var request = new LoginRequest();

            Assert.ThrowsAsync<BadRequestException>(() => testObj.LogIn(request));
        }

        [Test]
        public void LogIn_WrongPassword_NotFoundException()
        {
            const string email = "user@test.com";
            DbContextUtility.AddNew<ApplicationUser>(dbContext, u => u.Email = email);
            var request = new LoginRequest {Email = email};

            Assert.ThrowsAsync<BadRequestException>(() => testObj.LogIn(request));
        }

        [Test]
        public async Task UploadImage_Valid_SetsUserProfilePicKey()
        {
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);
            var file = PrepareCdnService();

            await testObj.UploadProfilePic(user.Id, file);

            Assert.AreEqual(ImageKey, user.ProfilePicKey);
        }

        [Test]
        public async Task UploadImage_Valid_ReturnImageUrl()
        {
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);
            var file = PrepareCdnService();

            var imageUrl = await testObj.UploadProfilePic(user.Id, file);

            Assert.AreEqual(ImageUrl, imageUrl);
        }

        [Test]
        public void UploadImage_NoUser_NotFoundException()
        {
            var file = PrepareCdnService();

            Assert.ThrowsAsync<NotFoundException>(() => testObj.UploadProfilePic("", file));
        }

        [Test]
        public void UploadImage_NoFile_BadRequestException()
        {
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);

            Assert.ThrowsAsync<BadRequestException>(() => testObj.UploadProfilePic(user.Id, null));
        }

        [Test]
        public async Task UploadImage_UserHasImage_DeleteOldImage()
        {
            const string firstKey = ImageKey + "_First";
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext, u => u.ProfilePicKey = firstKey);
            var file = PrepareCdnService();

            await testObj.UploadProfilePic(user.Id, file);

#pragma warning disable 4014
            cdnService.Received(1).DeleteImageAsync(firstKey, ClimbImageRules.ProfilePic);
#pragma warning restore 4014
        }

        [Test]
        public async Task UpdateSettings_NewUsername_UpdateUsername()
        {
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);
            const string username = "bob";
            var file = Substitute.For<IFormFile>();

            await testObj.UpdateSettings(user.Id, username, file);

            Assert.AreEqual(username, user.UserName);
        }

        [Test]
        public void UpdateSettings_NoUser_NotFoundException()
        {
            var file = Substitute.For<IFormFile>();

            Assert.ThrowsAsync<NotFoundException>(() => testObj.UpdateSettings("", "bob", file));
        }

        [Test]
        public async Task UpdateSettings_ProfilePic_UpdatePicture()
        {
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);
            var file = Substitute.For<IFormFile>();

            await testObj.UpdateSettings(user.Id, "bob", file);

#pragma warning disable 4014
            cdnService.Received(1).UploadImageAsync(file, ClimbImageRules.ProfilePic);
#pragma warning restore 4014
        }

        [Test]
        public async Task UpdateSettings_NoProfilePic_DontUpdatePicture()
        {
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);

            await testObj.UpdateSettings(user.Id, "bob", null);

#pragma warning disable 4014
            cdnService.DidNotReceiveWithAnyArgs().UploadImageAsync(null, null);
#pragma warning restore 4014
        }

        private IFormFile PrepareCdnService()
        {
            var file = Substitute.For<IFormFile>();
            cdnService.UploadImageAsync(file, ClimbImageRules.ProfilePic).Returns(ImageKey);
            cdnService.GetImageUrl(ImageKey, ClimbImageRules.ProfilePic).Returns(ImageUrl);
            return file;
        }
    }
}