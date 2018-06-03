using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
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

            IEmailSender emailSender = Substitute.For<IEmailSender>();
            ITokenHelper tokenHelper= Substitute.For<ITokenHelper>();
            IUrlUtility urlUtility= Substitute.For<IUrlUtility>();
            var signInManager = new FakeSignInManager();

            testObj = new ApplicationUserService(dbContext, cdnService, signInManager, emailSender, configuration, tokenHelper, urlUtility);
        }

        // TODO: LogIn
        // TODO: Register

        [Test]
        public async Task UploadImage_Valid_SetsUserProfilePicKey()
        {
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);
            var file = PrepareCdnService();

            var imageUrl = await testObj.UploadProfilePic(user.Id, file);

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

        private IFormFile PrepareCdnService()
        {
            var file = Substitute.For<IFormFile>();
            cdnService.UploadImageAsync(file, ClimbImageRules.ProfilePic).Returns(ImageKey);
            cdnService.GetImageUrl(ImageKey, ClimbImageRules.ProfilePic).Returns(ImageUrl);
            return file;
        }
    }
}