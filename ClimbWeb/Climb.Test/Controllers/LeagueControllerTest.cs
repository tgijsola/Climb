using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Models;
using Climb.Requests.Leagues;
using Climb.Services.Repositories;
using Climb.Test.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class LeagueControllerTest
    {
        private const string LeagueName = "NewLeague";

        private LeagueController testObj;
        private ILeagueRepository leagueRepository;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            leagueRepository = Substitute.For<ILeagueRepository>();
            dbContext = DbContextUtility.CreateMockDb();
            
            testObj = new LeagueController(leagueRepository, dbContext);
        }

        [Test]
        public async Task Create_Valid_CreatedResult()
        {
            var gameID = CreateGame().ID;
            var request = new CreateRequest {Name = LeagueName, GameID = gameID};

            leagueRepository.Create(LeagueName, gameID).Returns(new League {Name = LeagueName, GameID = gameID});

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
        }

        [Test]
        public async Task Create_NoGame_NotFound()
        {
            var request = new CreateRequest {Name = LeagueName, GameID = 0};

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Test]
        public async Task Create_NameTaken_Conflict()
        {
            var gameID = CreateGame().ID;
            var request = new CreateRequest {Name = LeagueName, GameID = gameID};
            
            dbContext.Add(new League {Name = LeagueName, GameID = gameID});
            dbContext.SaveChanges();

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status409Conflict, result.StatusCode);
        }

        private Game CreateGame()
        {
            var game = dbContext.Games.Add(new Game());
            dbContext.SaveChanges();
            return game.Entity;
        }
    }
}