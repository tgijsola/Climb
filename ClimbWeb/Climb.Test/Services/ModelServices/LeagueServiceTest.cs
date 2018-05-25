using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Models;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
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
            dbContext = DbContextUtility.CreateMockDb();

            testObj = new LeagueService(dbContext);
        }

        [Test]
        public async Task Create_Valid_ReturnLeague()
        {
            var game = DbContextUtility.AddNew<Game>(dbContext);

            var league = await testObj.Create("", game.ID);

            Assert.IsNotNull(league);
        }

        [Test]
        public void Create_NoGame_NotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.Create("", 0));
        }

        [Test]
        public void Create_NameTaken_Conflict()
        {
            var league = LeagueUtility.CreateLeague(dbContext);

            Assert.ThrowsAsync<ConflictException>(() => testObj.Create(league.Name, league.GameID));
        }

        [Test]
        public async Task Join_NewUser_CreateLeagueUser()
        {
            var game = DbContextUtility.AddNew<Game>(dbContext);
            var league = DbContextUtility.AddNew<League>(dbContext, l => l.GameID = game.ID);
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);

            var leagueUser = await testObj.Join(league.ID, user.Id);

            Assert.IsNotNull(leagueUser);
        }

        [Test]
        public async Task Join_OldUser_HasLeftFalse()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);

            var oldLeagueUser = new LeagueUser(league.ID, user.Id) {HasLeft = true};
            dbContext.LeagueUsers.Add(oldLeagueUser);
            dbContext.SaveChanges();

            var leagueUser = await testObj.Join(league.ID, user.Id);

            Assert.IsFalse(leagueUser.HasLeft);
            Assert.AreEqual(oldLeagueUser.ID, leagueUser.ID);
        }

        [Test]
        public void Join_NoLeague_DbException()
        {
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);

            Assert.ThrowsAsync<DbUpdateException>(() => testObj.Join(0, user.Id));
        }
    }
}