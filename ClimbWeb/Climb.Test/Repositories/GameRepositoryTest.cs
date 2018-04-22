using System.Threading.Tasks;
using Climb.Data;
using Climb.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Climb.Test.Repositories
{
    [TestFixture]
    public class GameRepositoryTest
    {
        private GameRepository testObj;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;

            dbContext = new ApplicationDbContext(options);

            testObj = new GameRepository(dbContext);
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