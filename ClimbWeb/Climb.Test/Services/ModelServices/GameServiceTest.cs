using System.Threading.Tasks;
using Climb.Data;
using Climb.Services.ModelServices;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Climb.Test.Services.ModelServices
{
    [TestFixture]
    public class GameServiceTest
    {
        private GameService testObj;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;

            dbContext = new ApplicationDbContext(options);

            testObj = new GameService(dbContext);
        }

        [Test]
        public async Task Create_Valid_ReturnGame()
        {
            const string name = "NewGame";

            var game = await testObj.Create(name);

            Assert.AreEqual(name, game.Name);
        }
    }
}