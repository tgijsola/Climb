using System.Net;
using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Extensions;
using Climb.Models;
using Climb.Requests.Leagues;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class LeagueControllerTest
    {
        private const string LeagueName = "NewLeague";

        private LeagueController testObj;
        private ILeagueService leagueService;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            leagueService = Substitute.For<ILeagueService>();
            dbContext = DbContextUtility.CreateMockDb();

            testObj = new LeagueController(leagueService, dbContext, Substitute.For<ILogger<LeagueController>>());
        }

        [Test]
        public async Task Create_Valid_CreatedResult()
        {
            var gameID = DbContextUtility.AddNew<Game>(dbContext).ID;
            var request = new CreateRequest {Name = LeagueName, GameID = gameID};

            leagueService.Create(LeagueName, gameID).Returns(new League {Name = LeagueName, GameID = gameID});

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Created);
        }

        [Test]
        public async Task Create_NoGame_NotFound()
        {
            var request = new CreateRequest {Name = LeagueName, GameID = 0};

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Create_NameTaken_Conflict()
        {
            var gameID = DbContextUtility.AddNew<Game>(dbContext).ID;
            var request = new CreateRequest {Name = LeagueName, GameID = gameID};

            dbContext.Add(new League {Name = LeagueName, GameID = gameID});
            dbContext.SaveChanges();

            var result = await testObj.Create(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Conflict);
        }

        [Test]
        public async Task Join_Valid_Created()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);

            var request = new JoinRequest(league.ID, user.Id);

            var result = await testObj.Join(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.Created);
        }

        [Test]
        public async Task Join_NoLeague_NotFound()
        {
            var request = new JoinRequest();

            var result = await testObj.Join(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.BadRequest);
        }
        
        [Test]
        public async Task Join_NoUser_NotFound()
        {
            var league = LeagueUtility.CreateLeague(dbContext);

            var request = new JoinRequest(league.ID, "");

            var result = await testObj.Join(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Get_Valid_Ok()
        {
            var league = LeagueUtility.CreateLeague(dbContext);

            var result = await testObj.Get(league.ID);
            var resultLeague = result.GetObject<League>();

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.OK);
            Assert.IsNotNull(resultLeague);
        }

        [Test]
        public async Task Get_NoLeague_NotFound()
        {
            var result = await testObj.Get(0);
            
            ControllerUtility.AssertStatusCode(result, HttpStatusCode.NotFound);
        }
    }
}