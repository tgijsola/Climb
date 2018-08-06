using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Models;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using NUnit.Framework;

namespace Climb.Test.Services.ModelServices
{
    // not owner
    // not admin
    [TestFixture]
    public class OrganizationServiceTest
    {
        private OrganizationService testObj;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();

            testObj = new OrganizationService(dbContext);
        }

        [Test]
        public void AddLeague_NoOrganization_NotFoundException()
        {
            var league = LeagueUtility.CreateLeague(dbContext);

            Assert.ThrowsAsync<NotFoundException>(() => testObj.AddLeague(0, league.ID));
        }

        [Test]
        public void AddLeague_NoLeague_NotFoundException()
        {
            var organization = DbContextUtility.AddNew<Organization>(dbContext);

            Assert.ThrowsAsync<NotFoundException>(() => testObj.AddLeague(organization.ID, 0));
        }

        [Test]
        public async Task AddLeague_NewLeague_LeagueAdded()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var organization = DbContextUtility.AddNew<Organization>(dbContext);

            await testObj.AddLeague(organization.ID, league.ID);

            Assert.AreSame(league, organization.Leagues[0]);
        }

        [Test]
        public async Task AddLeague_OldLeague_LeagueNotAdded()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var organization = DbContextUtility.AddNew<Organization>(dbContext);

            await testObj.AddLeague(organization.ID, league.ID);
            await testObj.AddLeague(organization.ID, league.ID);

            Assert.AreEqual(1, organization.Leagues.Count);
        }
        
        [Test]
        public async Task AddLeague_LeagueInAnotherOrg_LeagueNotAdded()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var organization1 = DbContextUtility.AddNew<Organization>(dbContext);
            var organization2 = DbContextUtility.AddNew<Organization>(dbContext);

            await testObj.AddLeague(organization1.ID, league.ID);
            await testObj.AddLeague(organization2.ID, league.ID);

            Assert.AreEqual(0, organization2.Leagues.Count);
        }
    }
}