using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Models;
using Climb.Requests.Seasons;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class SeasonControllerTest
    {
        private SeasonController testObj;
        private ISeasonService seasonService;
        private ApplicationDbContext dbContext;

        private int gameID;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();
            var game = DbContextUtility.AddNew<Game>(dbContext);
            gameID = game.ID;

            seasonService = Substitute.For<ISeasonService>();

            testObj = new SeasonController(seasonService, dbContext, Substitute.For<ILogger<SeasonController>>());
        }

        [Test]
        public async Task ListForLeague_NoLeague_NotFound()
        {
            var result = await testObj.ListForLeague(0);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task ListForLeague_Valid_ReturnOk()
        {
            var league = DbContextUtility.AddNew<League>(dbContext, l => l.GameID = gameID);

            var result = await testObj.ListForLeague(league.ID);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.OK);
        }

        [Test]
        public async Task Create_Valid_ReturnOk()
        {
            var league = DbContextUtility.AddNew<League>(dbContext, l => l.GameID = gameID);
            var request = new CreateRequest(league.ID, DateTime.Now.AddMinutes(1), DateTime.Now.AddMinutes(2));

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Created);
        }

        [Test]
        public async Task Create_NoLeague_ReturnNotFound()
        {
            var request = new CreateRequest(0, DateTime.Now.AddMinutes(1), DateTime.Now.AddMinutes(2));

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Create_StartInPast_ReturnBadRequest()
        {
            var league = DbContextUtility.AddNew<League>(dbContext, l => l.GameID = gameID);
            var request = new CreateRequest(league.ID, DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(2));

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Create_EndBeforeStart_ReturnBadRequest()
        {
            var league = DbContextUtility.AddNew<League>(dbContext, l => l.GameID = gameID);
            var request = new CreateRequest(league.ID, DateTime.Now.AddMinutes(3), DateTime.Now.AddMinutes(2));

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.BadRequest);
        }
    }
}