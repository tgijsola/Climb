using System.Net;
using System.Threading.Tasks;
using Climb.API;
using Climb.Data;
using Climb.Extensions;
using Climb.Models;
using Climb.Requests.Games;
using Climb.Services.ModelServices;
using Climb.Test.Fakes;
using Climb.Test.Utilities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Api
{
    [TestFixture]
    public class GameApiTest
    {
        private GameApi testObj;
        private IGameService gameService;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            gameService = Substitute.For<IGameService>();
            dbContext = DbContextUtility.CreateMockDb();
            var userManager = new FakeUserManager();
            var logger = Substitute.For<ILogger<GameApi>>();

            testObj = new GameApi(logger, dbContext, gameService);
        }

        [Test]
        public async Task Get_HasGame_Ok()
        {
            var game = GameUtility.Create(dbContext, 2, 2);

            var result = await testObj.Get(game.ID);
            var resultObj = result.GetObject<Game>();

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.OK);
            Assert.AreEqual(game.ID, resultObj.ID);
        }

        [Test]
        public async Task Get_NoGame_NotFound()
        {
            var result = await testObj.Get(0);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Create_Valid_CreatedResult()
        {
            var request = new CreateRequest();

            gameService.Create(request).Returns(new Game());

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Created);
        }

        [Test]
        public async Task AddCharacter_Valid_Created()
        {
            var game = GameUtility.Create(dbContext, 0, 0);
            var request = new AddCharacterRequest(game.ID, "Char1");
            gameService.AddCharacter(request).Returns(new Character());

            var result = await testObj.AddCharacter(request);
            var resultObj = result.GetObject<Character>();

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Created);
            Assert.IsNotNull(resultObj);
        }

        [Test]
        public async Task AddStage_Valid_Created()
        {
            var game = GameUtility.Create(dbContext, 0, 0);
            var request = new AddStageRequest(game.ID, "Stage1");
            gameService.AddStage(request).Returns(new Stage());

            var result = await testObj.AddStage(request);
            var resultObj = result.GetObject<Stage>();

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Created);
            Assert.IsNotNull(resultObj);
        }
    }
}