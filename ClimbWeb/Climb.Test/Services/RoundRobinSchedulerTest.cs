using System.Collections.Generic;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using Climb.Services;
using Climb.Test.Utilities;
using NUnit.Framework;

namespace Climb.Test.Services
{
    // TODO: Sets are spaced out.
    // TODO: Everyone fights everyone.
    public class RoundRobinSchedulerTest
    {
        private RoundRobinScheduler testObj;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();

            testObj = new RoundRobinScheduler();
        }

        [TestCase(2, 1)]
        [TestCase(4, 6)]
        [TestCase(11, 55)]
        public async Task GenerateSchedule_Valid_CreateSets(int userCount, int setCount)
        {
            var season = SeasonUtility.CreateSeason(dbContext, userCount);
            season.Sets = new HashSet<Set>();

            await testObj.GenerateScheduleAsync(season, dbContext);

            Assert.AreEqual(setCount, season.Sets.Count);
        }
    }
}