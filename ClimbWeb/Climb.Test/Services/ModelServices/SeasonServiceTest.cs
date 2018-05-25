using System;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
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
        private ScheduleFactory scheduler;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();
            scheduler = Substitute.For<ScheduleFactory>();

            testObj = new SeasonService(dbContext, scheduler);
        }

        [Test]
        public async Task Create_Valid_NotNull()
        {
            var league = LeagueUtility.CreateLeague(dbContext);

            var season = await testObj.Create(league.ID, DateTime.MaxValue, DateTime.MaxValue);

            Assert.IsNotNull(season);
        }

        [Test]
        public void Create_NoLeague_NotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.Create(0, DateTime.MaxValue, DateTime.MaxValue));
        }

        [Test]
        public async Task Create_Valid_AddsMembers()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            LeagueUtility.AddUsersToLeague(league, 1, dbContext);

            var season = await testObj.Create(league.ID, DateTime.MaxValue, DateTime.MaxValue);

            Assert.IsTrue(season.Participants.Count > 0);
        }
    }
}