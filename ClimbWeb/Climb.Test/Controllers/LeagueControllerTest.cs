using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Models;
using Climb.Requests.Leagues;
using Climb.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class LeagueControllerTest
    {
        private const string Name = "NewLeague";
        private const int GameID = 2;

        private LeagueController testObj;
        private ILeagueRepository leagueRepository;
        private IGameRepository gameRepository;

        [SetUp]
        public void SetUp()
        {
            leagueRepository = Substitute.For<ILeagueRepository>();
            gameRepository = Substitute.For<IGameRepository>();

            testObj = new LeagueController(leagueRepository, gameRepository);
        }

        [Test]
        public async Task Create_Valid_CreatedResult()
        {
            var request = new CreateRequest {Name = Name, GameID = GameID};

            gameRepository.Any(null).ReturnsForAnyArgs(true);
            leagueRepository.Create(Name, GameID).Returns(new League {Name = Name, GameID = GameID});

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
        }

        [Test]
        public async Task Create_NoGame_NotFound()
        {
            var request = new CreateRequest {Name = Name, GameID = GameID};

            gameRepository.Any(null).ReturnsForAnyArgs(false);

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Test]
        public async Task Create_NameTaken_Conflict()
        {
            var request = new CreateRequest {Name = Name, GameID = GameID};

            gameRepository.Any(null).ReturnsForAnyArgs(true);
            leagueRepository.Any(null).ReturnsForAnyArgs(true);

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status409Conflict, result.StatusCode);
        }
    }
}