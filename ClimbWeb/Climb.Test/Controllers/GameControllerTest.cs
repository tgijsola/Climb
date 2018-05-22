using System.Net;
using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Exceptions;
using Climb.Extensions;
using Climb.Models;
using Climb.Requests.Games;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class GameControllerTest
    {
        private GameController testObj;
        private IGameService gameService;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            gameService = Substitute.For<IGameService>();
            dbContext = DbContextUtility.CreateMockDb();

            testObj = new GameController(gameService, dbContext, Substitute.For<ILogger<GameController>>());
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
        public async Task Create_NameTaken_BadRequest()
        {
            const string name = "NewGame";
            var request = new CreateRequest {Name = name};

            dbContext.Games.Add(new Game {Name = name});
            dbContext.SaveChanges();

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.BadRequest);
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
        public async Task AddCharacter_NotFound_NotFound()
        {
            gameService.AddCharacter(null).ThrowsForAnyArgs(new NotFoundException());

            var result = await testObj.AddCharacter(new AddCharacterRequest());

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task AddCharacter_BadRequest_BadRequest()
        {
            gameService.AddCharacter(null).ThrowsForAnyArgs(new BadRequestException());

            var result = await testObj.AddCharacter(new AddCharacterRequest());

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.BadRequest);
        }
    }
}