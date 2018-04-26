using System;
using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Models;
using Climb.Requests.Seasons;
using Climb.Services.Repositories;
using Climb.Test.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class SeasonControllerTest
    {
        private SeasonController testObj;
        private ISeasonRepository seasonRepository;
        private ApplicationDbContext dbContext;

        private int gameID;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();
            var game = dbContext.Games.Add(new Game());
            dbContext.SaveChanges();
            gameID = game.Entity.ID;

            seasonRepository = Substitute.For<ISeasonRepository>();

            testObj = new SeasonController(seasonRepository, dbContext);
        }

        [Test]
        public async Task ListForLeague_NoLeague_NotFound()
        {
            var result = (ObjectResult)await testObj.ListForLeague(0);

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Test]
        public async Task ListForLeague_Valid_ReturnOk()
        {
            var league = CreateLeague();

            var result = (ObjectResult)await testObj.ListForLeague(league.ID);

            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [Test]
        public async Task Create_Valid_ReturnOk()
        {
            var league = CreateLeague();
            var request = new CreateRequest(league.ID, DateTime.Now.AddMinutes(1), DateTime.Now.AddMinutes(2));

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
        }

        [Test]
        public async Task Create_NoLeague_ReturnNotFound()
        {
            var request = new CreateRequest(0, DateTime.Now.AddMinutes(1), DateTime.Now.AddMinutes(2));

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Test]
        public async Task Create_StartInPast_ReturnBadRequest()
        {
            var league = CreateLeague();
            var request = new CreateRequest(league.ID, DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(2));

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Test]
        public async Task Create_EndBeforeStart_ReturnBadRequest()
        {
            var league = CreateLeague();
            var request = new CreateRequest(league.ID, DateTime.Now.AddMinutes(3), DateTime.Now.AddMinutes(2));

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        private League CreateLeague()
        {
            var league = new League {GameID = gameID};
            dbContext.Leagues.Add(league);
            dbContext.SaveChanges();
            return league;
        }
    }
}