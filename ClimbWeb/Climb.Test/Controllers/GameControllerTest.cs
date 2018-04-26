﻿using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Models;
using Climb.Requests.Games;
using Climb.Services.Repositories;
using Climb.Test.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class GameControllerTest
    {
        private GameController testObj;
        private IGameRepository gameRepository;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            gameRepository = Substitute.For<IGameRepository>();
            dbContext = DbContextUtility.CreateMockDb();

            testObj = new GameController(gameRepository, dbContext);
        }

        [Test]
        public async Task Create_Valid_CreatedResult()
        {
            const string name = "NewGame";
            var request = new CreateRequest {Name = name};

            gameRepository.Create(name).Returns(new Game {Name = name});

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
        }

        [Test]
        public async Task Create_NameTaken_BadRequest()
        {
            const string name = "NewGame";
            var request = new CreateRequest {Name = name};

            dbContext.Games.Add(new Game {Name = name});
            dbContext.SaveChanges();

            var result = (ObjectResult)await testObj.Create(request);

            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}