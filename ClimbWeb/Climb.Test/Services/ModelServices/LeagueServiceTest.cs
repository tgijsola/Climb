using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Models;
using Climb.Services;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Services.ModelServices
{
    [TestFixture]
    public class LeagueServiceTest
    {
        private const int StartingPoints = 2000;

        private LeagueService testObj;
        private ApplicationDbContext dbContext;
        private IPointService pointService;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();
            pointService = Substitute.For<IPointService>();

            testObj = new LeagueService(dbContext, pointService);
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

            var oldLeagueUser = new LeagueUser(league.ID, user.Id)
            {
                HasLeft = true
            };
            dbContext.LeagueUsers.Add(oldLeagueUser);
            dbContext.SaveChanges();

            var leagueUser = await testObj.Join(league.ID, user.Id);

            Assert.IsFalse(leagueUser.HasLeft);
            Assert.AreEqual(oldLeagueUser.ID, leagueUser.ID);
        }

        [Test]
        public void Join_NoLeague_NotFound()
        {
            var user = DbContextUtility.AddNew<ApplicationUser>(dbContext);

            Assert.ThrowsAsync<NotFoundException>(() => testObj.Join(0, user.Id));
        }

        [Test]
        public void Join_NoUser_NotFound()
        {
            var league = LeagueUtility.CreateLeague(dbContext);

            Assert.ThrowsAsync<NotFoundException>(() => testObj.Join(league.ID, ""));
        }

        [Test]
        public void TakeSnapshots_NoLeague_NotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.TakeSnapshots(0));
        }

        [Test]
        public async Task TakeSnapshots_Season_ReturnSnapshots()
        {
            var season = CreateSeason();
            CreateSets(season.League, season);

            var snapshots = await testObj.TakeSnapshots(season.LeagueID);

            Assert.AreEqual(season.League.Members.Count, snapshots.Count);
        }

        [Test]
        public async Task TakeSnapshots_SeasonAndExhibition_ReturnSnapshots()
        {
            var season = CreateSeason();
            CreateSets(season.League, null);
            CreateSets(season.League, season);

            var snapshots = await testObj.TakeSnapshots(season.LeagueID);

            Assert.AreEqual(season.League.Members.Count, snapshots.Count);
        }

        [Test]
        public async Task TakeSnapshots_Season_StartingValuesAreCorrect()
        {
            var season = CreateSeason();
            CreateSets(season.League, season);

            var snapshots = await testObj.TakeSnapshots(season.LeagueID);

            foreach(var snapshot in snapshots)
            {
                Assert.AreEqual(snapshot.LeagueUser.Rank, snapshot.Rank);
                Assert.AreEqual(snapshot.LeagueUser.Points, snapshot.Points);
            }
        }

        private Season CreateSeason()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var members = LeagueUtility.AddUsersToLeague(league, 10, dbContext);
            for(var i = 0; i < members.Count; i++)
            {
                var member = members[i];
                member.Points = StartingPoints - i;
                member.Rank = i + 1;
            }

            var season = DbContextUtility.AddNew<Season>(dbContext, s => s.League = league);
            return season;
        }

        private void CreateSets(League league, Season season)
        {
            var firstMember = league.Members[0];
            for(var i = 1; i < league.Members.Count; i++)
            {
                var nextMember = league.Members[i].ID;
                var set = SetUtility.Create(dbContext, firstMember.ID, nextMember, league.ID, season);
                set.IsComplete = true;
                set.Player1Score = 2;
                set.Player2Score = 1;
            }
        }
    }
}