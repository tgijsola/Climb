using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Climb.Core.TieBreakers;
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
    public class SeasonServiceTest
    {
        private SeasonService testObj;
        private ApplicationDbContext dbContext;
        private IScheduleFactory scheduler;
        private ISeasonPointCalculator pointCalculator;
        private ITieBreaker tieBreaker;

        private static DateTime StartDate => DateTime.Now.AddDays(1);
        private static DateTime EndDate => DateTime.Now.AddDays(2);

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();
            scheduler = Substitute.For<IScheduleFactory>();
            pointCalculator = Substitute.For<ISeasonPointCalculator>();
            var tieBreakerFactory = Substitute.For<ITieBreakerFactory>();
            tieBreaker = Substitute.For<TieBreaker>();

            tieBreakerFactory.Create().Returns(tieBreaker);

            testObj = new SeasonService(dbContext, scheduler, pointCalculator, tieBreakerFactory);
        }

        [Test]
        public async Task Create_Valid_NotNull()
        {
            var league = LeagueUtility.CreateLeague(dbContext);

            var season = await testObj.Create(league.ID, StartDate, EndDate);

            Assert.IsNotNull(season);
        }

        [Test]
        public void Create_NoLeague_NotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.Create(0, StartDate, EndDate));
        }

        [Test]
        public void Create_StartInPast_BadRequestException()
        {
            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(0, DateTime.MinValue, EndDate));
        }

        [Test]
        public void Create_EndBeforeStart_BadRequestException()
        {
            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(0, DateTime.Now.AddDays(2), StartDate));
        }

        [Test]
        public async Task Create_Valid_AddsMembers()
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            LeagueUtility.AddUsersToLeague(league, 1, dbContext);

            var season = await testObj.Create(league.ID, StartDate, EndDate);

            Assert.IsTrue(season.Participants.Count > 0);
        }

        [Test]
        public void GenerateSchedule_NoSeason_NotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.GenerateSchedule(0));
        }

        [Test]
        public async Task UpdateStandings_Valid_PointsAssigned()
        {
            var (winnerPoints, loserPoints) = (2, 1);
            var set = CreateSet(loserPoints, winnerPoints);

            await testObj.UpdateStandings(set.ID);

            Assert.AreEqual(loserPoints, set.Season.Participants.First(slu => slu.LeagueUserID == set.Player1ID).Points);
            Assert.AreEqual(winnerPoints, set.Season.Participants.First(slu => slu.LeagueUserID == set.Player2ID).Points);
        }

        [Test]
        public async Task UpdateStandings_Valid_PointsSaved()
        {
            var (winnerPoints, loserPoints) = (2, 1);
            var set = CreateSet(loserPoints, winnerPoints);

            await testObj.UpdateStandings(set.ID);

            Assert.AreEqual(loserPoints, set.Player1SeasonPoints);
            Assert.AreEqual(winnerPoints, set.Player2SeasonPoints);
        }

        [Test]
        public async Task UpdateStandings_NoTies_RanksUpdated()
        {
            var (winnerPoints, loserPoints) = (2, 1);
            var set = CreateSet(loserPoints, winnerPoints);

            await testObj.UpdateStandings(set.ID);

            Assert.AreEqual(1, set.SeasonPlayer2.Standing);
            Assert.AreEqual(2, set.SeasonPlayer1.Standing);
        }

        [Test]
        public async Task UpdateStandings_Ties_NoTiedStandings()
        {
            tieBreaker.WhenForAnyArgs(tb => tb.Break(null)).Do(info =>
            {
                var arg = info.Arg<Dictionary<IParticipant, ParticipantRecord>>();
                var points = 1;
                foreach(var entry in arg)
                {
                    entry.Key.TieBreakerPoints = points;
                    ++points;
                }
            });

            var season = SeasonUtility.CreateSeason(dbContext, 4).season;
            foreach(var seasonLeagueUser in season.Participants)
            {
                seasonLeagueUser.Standing = 1;
            }

            var set = SetUtility.Create(dbContext, season.Participants[0], season.Participants[1], season.LeagueID);

            await testObj.UpdateStandings(set.ID);

            season.Participants.Sort();
            for(int i = 0; i < season.Participants.Count; i++)
            {
                Assert.AreEqual(i + 1, season.Participants[i].Standing);
            }
        }

        private Set CreateSet(int p1Score, int p2Score)
        {
            var winnerScore = p1Score > p2Score ? p1Score : p2Score;
            var loserScore = p1Score > p2Score ? p2Score : p1Score;

            pointCalculator.CalculatePointDeltas(null, null).ReturnsForAnyArgs((winnerScore, loserScore));
            var season = SeasonUtility.CreateSeason(dbContext, 2).season;
            var set = SeasonUtility.CreateSets(dbContext, season)[0];
            set.Player1Score = p1Score;
            set.Player2Score = p2Score;

            return set;
        }
    }
}