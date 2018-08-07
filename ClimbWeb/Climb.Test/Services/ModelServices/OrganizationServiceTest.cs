using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Models;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using NUnit.Framework;

namespace Climb.Test.Services.ModelServices
{
    // not admin
    [TestFixture]
    public class OrganizationServiceTest
    {
        private OrganizationService testObj;
        private ApplicationDbContext dbContext;
        private string userID;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();

            testObj = new OrganizationService(dbContext);

            userID = DbContextUtility.AddNew<ApplicationUser>(dbContext).Id;
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void AddLeague_NoOrganization_NotFoundException()
        {
            var league = LeagueUtility.CreateLeague(dbContext);

            Assert.ThrowsAsync<NotFoundException>(() => testObj.AddLeague(0, league.ID, userID));
        }

        [Test]
        public void AddLeague_NoLeague_NotFoundException()
        {
            var organization = DbContextUtility.AddNew<Organization>(dbContext);
            MakeUserAnOwner(organization, userID);

            Assert.ThrowsAsync<NotFoundException>(() => testObj.AddLeague(organization.ID, 0, userID));
        }

        [Test]
        public async Task AddLeague_NewLeague_LeagueAdded()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var organization = DbContextUtility.AddNew<Organization>(dbContext);
            MakeUserAnOwner(organization, userID);

            await testObj.AddLeague(organization.ID, league.ID, userID);

            Assert.AreSame(league, organization.Leagues[0]);
        }

        [Test]
        public async Task AddLeague_OldLeague_LeagueNotAdded()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var organization = DbContextUtility.AddNew<Organization>(dbContext);
            MakeUserAnOwner(organization, userID);

            await testObj.AddLeague(organization.ID, league.ID, userID);
            await testObj.AddLeague(organization.ID, league.ID, userID);

            Assert.AreEqual(1, organization.Leagues.Count);
        }
        
        [Test]
        public async Task AddLeague_LeagueInAnotherOrg_LeagueNotAdded()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var organization1 = DbContextUtility.AddNew<Organization>(dbContext);
            MakeUserAnOwner(organization1, userID);
            var organization2 = DbContextUtility.AddNew<Organization>(dbContext);
            MakeUserAnOwner(organization2, userID);

            await testObj.AddLeague(organization1.ID, league.ID, userID);
            await testObj.AddLeague(organization2.ID, league.ID, userID);

            Assert.AreEqual(0, organization2.Leagues.Count);
        }

        [Test]
        public void AddLeague_NotOwner_NotAuthorizedException()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var organization = DbContextUtility.AddNew<Organization>(dbContext);

            Assert.ThrowsAsync<NotAuthorizedException>(() => testObj.AddLeague(organization.ID, league.ID, ""));
        }

        #region Helpers
        private void MakeUserAnOwner(Organization organization, string userID)
        {
            DbContextUtility.AddNew<OrganizationUser>(dbContext, ou =>
            {
                ou.UserID = userID;
                ou.OrganizationID = organization.ID;
                ou.IsOwner = true;
            });
        }
        #endregion
    }
}