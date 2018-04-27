using System.Net;
using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Models;
using Climb.Requests.Leagues;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class LeagueControllerTest
    {
        private const string LeagueName = "NewLeague";

        private LeagueController testObj;
        private ILeagueService leagueService;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            leagueService = Substitute.For<ILeagueService>();
            dbContext = DbContextUtility.CreateMockDb();

            testObj = new LeagueController(leagueService, dbContext, Substitute.For<ILogger<LeagueController>>());
        }

        [Test]
        public async Task Create_Valid_CreatedResult()
        {
            var gameID = CreateGame().ID;
            var request = new CreateRequest {Name = LeagueName, GameID = gameID};

            leagueService.Create(LeagueName, gameID).Returns(new League {Name = LeagueName, GameID = gameID});

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Created);
        }

        [Test]
        public async Task Create_NoGame_NotFound()
        {
            var request = new CreateRequest {Name = LeagueName, GameID = 0};

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Create_NameTaken_Conflict()
        {
            var gameID = CreateGame().ID;
            var request = new CreateRequest {Name = LeagueName, GameID = gameID};

            dbContext.Add(new League {Name = LeagueName, GameID = gameID});
            dbContext.SaveChanges();

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Conflict);
        }

        private Game CreateGame()
        {
            var game = dbContext.Games.Add(new Game());
            dbContext.SaveChanges();
            return game.Entity;
        }
    }
}