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
            League league = CreateLeague();

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
            League league = CreateLeague();
            JoinUser(league);

            var season = await testObj.Create(league.ID, DateTime.MaxValue, DateTime.MaxValue);

            Assert.IsTrue(season.Participants.Count > 0);
        }

        [TestCase(2, 1)]
        [TestCase(4, 6)]
        [TestCase(11, 55)]
        public async Task GenerateSchedule_Valid_CreateSets(int userCount, int setCount)
        {
            var league = CreateLeague();
            AddUserToLeague(league, userCount);
            var season = await testObj.Create(league.ID, DateTime.Now.AddMinutes(1), DateTime.Now.AddMinutes(2));

            await testObj.GenerateSchedule(season.ID);

            Assert.AreEqual(setCount, season.Sets.Count);
        }

        [TestCase(0)]
        [TestCase(1)]
        public async Task GenerateSchedule_NotEnoughParticipants_Exception(int userCount)
        {
            var league = CreateLeague();
            AddUserToLeague(league, userCount);
            var season = await testObj.Create(league.ID, DateTime.Now.AddMinutes(1), DateTime.Now.AddMinutes(2));

            Assert.ThrowsAsync<InvalidOperationException>(() => testObj.GenerateSchedule(season.ID));
        }

        // TODO: Already started.
        // TODO: Already ended.
        // TODO: Already has sets.

        private League CreateLeague()
        {
            var game = DbContextUtility.AddNew<Game>(dbContext);
            var league = DbContextUtility.AddNew<League>(dbContext, l => l.GameID = game.ID);
            return league;
        }

        private void AddUserToLeague(League league, int count)
        {
            for (int i = 0; i < count; i++)
            {
                JoinUser(league);
            }
        }

        private void JoinUser(League league)
        {
            DbContextUtility.AddNew<LeagueUser>(dbContext, lu => lu.LeagueID = league.ID);
        }
    }
}