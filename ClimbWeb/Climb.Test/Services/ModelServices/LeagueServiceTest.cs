using System.Threading.Tasks;
using Climb.Data;
using Climb.Services.ModelServices;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Climb.Test.Services.ModelServices
{
    [TestFixture]
    public class LeagueServiceTest
    {
        private LeagueService testObj;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;

            dbContext = new ApplicationDbContext(options);

            testObj = new LeagueService(dbContext);
        }

        [Test]
        public async Task Create_Valid_ReturnLeague()
        {
            const string name = "NewLeague";
            const int gameID = 2;

            var league = await testObj.Create(name, gameID);

            Assert.AreEqual(name, league.Name);
            Assert.AreEqual(gameID, league.GameID);
        }
    }
}