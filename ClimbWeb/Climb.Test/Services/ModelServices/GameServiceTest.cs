using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Requests.Games;
using Climb.Exceptions;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Climb.Test.Services.ModelServices
{
    [TestFixture]
    public class GameServiceTest
    {
        private GameService testObj;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;

            dbContext = new ApplicationDbContext(options);

            testObj = new GameService(dbContext);
        }

        [Test]
        public async Task Create_Valid_ReturnGame()
        {
            var request = new CreateRequest("GameName", 1, 2);

            var game = await testObj.Create(request);

            Assert.IsNotNull(game);
        }

        [Test]
        public void Create_NameTaken_BadRequestException()
        {
            var game = GameUtility.Create(dbContext, 1, 1);
            var request = new CreateRequest(game.Name, 1, 2);

            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(request));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Create_InvalidMaxCharacters_BadRequestException(int maxCharacters)
        {
            var request = new CreateRequest("GameName", maxCharacters, 2);

            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(request));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Create_InvalidMaxMatchPoints_BadRequestException(int maxPoints)
        {
            var request = new CreateRequest("GameName", 1, maxPoints);

            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(request));
        }

        [Test]
        public void Create_NameTaken_BadRequest()
        {
            var game = GameUtility.Create(dbContext, 0, 0);
            var request = new CreateRequest(game.Name, 1, 1);


            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(request));
        }
    }
}