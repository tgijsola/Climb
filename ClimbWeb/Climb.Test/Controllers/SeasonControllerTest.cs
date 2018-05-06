using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Extensions;
using Climb.Models;
using Climb.Requests.Seasons;
using Climb.Responses.Seasons;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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
        public async Task Get_Valid_Ok()
        {
            var season = SeasonUtility.CreateSeason(dbContext, 2);

            var result = await testObj.Get(season.ID);
            var resultSeason = result.GetObject<GetResponse>();

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.OK);
            Assert.IsNotNull(resultSeason.Participants);
        }

        [Test]
        public async Task Get_NoSeason_NotFound()
        {
            var result = await testObj.Get(0);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.NotFound);
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

        [Test]
        public async Task Start_Valid_Created()
        {
            var season = SeasonUtility.CreateSeason(dbContext, 2);

            var result = await testObj.Start(season.ID);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Created);
        }

        [Test]
        public async Task Start_Valid_ReturnsSets()
        {
            var season = SeasonUtility.CreateSeason(dbContext, 2);
            var participants = season.Participants.ToArray();
            var sets = new HashSet<Set>();
            for(var i = 0; i < 3; i++)
            {
                sets.Add(SetUtility.Create(dbContext, participants[0].LeagueUserID, participants[1].LeagueUserID, season));
            }

            seasonService.GenerateSchedule(season.ID).ReturnsForAnyArgs(info => sets);

            var result = await testObj.Start(season.ID);
            var setResults = result.GetObject<ICollection<Set>>();

            Assert.IsTrue(setResults.Count == sets.Count);
        }

        [Test]
        public async Task Start_NoSeason_NotFound()
        {
            var result = await testObj.Start(0);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Start_ServiceError_ServerError()
        {
            var season = SeasonUtility.CreateSeason(dbContext, 2);
            seasonService.GenerateSchedule(season.ID).Throws<Exception>();

            var result = await testObj.Start(season.ID);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.InternalServerError);
        }
    }
}