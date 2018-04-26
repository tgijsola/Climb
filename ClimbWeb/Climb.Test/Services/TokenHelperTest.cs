using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Services;
using Climb.Services.ModelServices;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Services
{
    [TestFixture]
    public class TokenHelperTest
    {
        private const string Email = "email@test.com";
        private const string ID = "testId";

        private TokenHelper testObj;
        private IApplicationUserService userService;
        private ApplicationUser user;

        [SetUp]
        public void SetUp()
        {
            userService = Substitute.For<IApplicationUserService>();

            testObj = new TokenHelper(userService);

            user = new ApplicationUser {Id = ID, Email = Email};
        }

        [Test]
        public async Task GetUserID_Valid_ReturnsID()
        {
            userService.GetByEmail(Email).Returns(Task.FromResult(user));
            var token = CreateToken(Email);

            var userID = await testObj.GetAuthorizedUserID(token);

            Assert.AreEqual(ID, userID);
        }

        [Test]
        public async Task GetUserID_ValidWithPrefix_ReturnsIDAsync()
        {
            userService.GetByEmail(Email).Returns(Task.FromResult(user));
            var token = CreateToken(Email);
            token = $"Bearer {token}";

            var userID = await testObj.GetAuthorizedUserID(token);

            Assert.AreEqual(ID, userID);
        }

        [Test]
        public async Task GetUserID_ValidNoUserFound_EmptyStringAsync()
        {
            var token = CreateToken(Email);

            var userID = await testObj.GetAuthorizedUserID(token);

            Assert.IsEmpty(userID);
        }

        [Test]
        public async Task GetUserID_Invalid_EmptyString()
        {
            var userID = await testObj.GetAuthorizedUserID("Invalid Token");

            Assert.IsEmpty(userID);
        }

        private static string CreateToken(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email)
            };

            var token = new JwtSecurityToken(
                "climb.com",
                "climb",
                claims);

            var serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return serializedToken;
        }
    }
}