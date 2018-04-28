using System;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using NUnit.Framework;

namespace Climb.Test.Services.ModelServices
{
    [TestFixture]
    public class SeasonServiceTest
    {
        private SeasonService testObj;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();

            testObj = new SeasonService(dbContext);
        }

        [Test]
        public async Task Create_Valid_NotNull()
        {
            var game = DbContextUtility.AddNew<Game>(dbContext);
            var league = DbContextUtility.AddNew<League>(dbContext, l => l.GameID = game.ID);

            var season = await testObj.Create(league.ID, DateTime.MaxValue, DateTime.MaxValue);

            Assert.IsNotNull(season);
        }

        [Test]
        public void Create_NoLeague_InvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => testObj.Create(0, DateTime.MaxValue, DateTime.MaxValue));
        }

        [Test]
        public async Task Create_Valid_AddsMembers()
        {
            var game = DbContextUtility.AddNew<Game>(dbContext);
            var league = DbContextUtility.AddNew<League>(dbContext, l => l.GameID = game.ID);
            DbContextUtility.AddNew<LeagueUser>(dbContext, lu => lu.LeagueID = league.ID);

            var season = await testObj.Create(league.ID, DateTime.MaxValue, DateTime.MaxValue);

            Assert.IsTrue(season.Participants.Count > 0);
        }
    }
}