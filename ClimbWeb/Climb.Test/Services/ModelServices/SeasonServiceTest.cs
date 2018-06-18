using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Models;
using Climb.Services;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Services.ModelServices
{
    [TestFixture]
    public class SeasonServiceTest
    {
        private SeasonService testObj;
        private ApplicationDbContext dbContext;
        private IScheduleFactory scheduler;

        private static DateTime StartDate => DateTime.Now.AddDays(1);
        private static DateTime EndDate => DateTime.Now.AddDays(2);

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();
            scheduler = Substitute.For<IScheduleFactory>();

            testObj = new SeasonService(dbContext, scheduler);
        }

        [Test]
        public async Task Create_Valid_NotNull()
        {
            var league = LeagueUtility.CreateLeague(dbContext);

            var season = await testObj.Create(league.ID, StartDate, EndDate);

            Assert.IsNotNull(season);
        }

        [Test]
        public void Create_NoLeague_NotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.Create(0, StartDate, EndDate));
        }

        [Test]
        public void Create_StartInPast_BadRequestException()
        {
            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(0, DateTime.MinValue, EndDate));
        }

        [Test]
        public void Create_EndBeforeStart_BadRequestException()
        {
            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(0, DateTime.Now.AddDays(2), StartDate));
        }

        [Test]
        public async Task Create_Valid_AddsMembers()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            LeagueUtility.AddUsersToLeague(league, 1, dbContext);

            var season = await testObj.Create(league.ID, StartDate, EndDate);

            Assert.IsTrue(season.Participants.Count > 0);
        }

        [Test]
        public void GenerateSchedule_NoSeason_NotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.GenerateSchedule(0));
        }
    }
}