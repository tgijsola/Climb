using System;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Requests.Sets;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using NUnit.Framework;

namespace Climb.Test.Services.ModelServices
{
    [TestFixture]
    public class SetServiceTest
    {
        private SetService testObj;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();
            testObj = new SetService(dbContext);
        }

        [Test]
        public async Task Update_FirstMatches_CreatesMatchesAndMatchCharacters()
        {
            var set = SetUtility.Create(dbContext);

            var matchForms = CreateMatchForms(3);
            await testObj.Update(set.ID, matchForms);

            Assert.AreEqual(3, set.Matches.Count);
        }

        [Test]
        public async Task Update_NewMatches_ReplacesOldMatchesAndMatchCharacters()
        {
            var set = SetUtility.Create(dbContext);
            var matchForms = CreateMatchForms(3);
            await testObj.Update(set.ID, matchForms);

            await testObj.Update(set.ID, matchForms);

            Assert.AreEqual(3, set.Matches.Count);
        }

        [Test]
        public async Task Update_RemoveMatches_DeletesOldMatchesAndMatchCharacters()
        {
            var set = SetUtility.Create(dbContext);
            var matchForms = CreateMatchForms(3);
            await testObj.Update(set.ID, matchForms);

            await testObj.Update(set.ID, new MatchForm[0]);

            Assert.AreEqual(0, set.Matches.Count);
        }

        [Test]
        public void Update_NoSet_NullRefException()
        {
            var matchForms = CreateMatchForms(3);
            Assert.ThrowsAsync<NullReferenceException>(() => testObj.Update(0, matchForms));
        }

        // TODO: Not matching character counts.

        private static MatchForm[] CreateMatchForms(int count)
        {
            var matchForms = new MatchForm[count];
            for(int i = 0; i < count; i++)
            {
                matchForms[i] = new MatchForm
                {
                    Player1Score = 1,
                    Player2Score = 2,
                    Player1Characters = new[] {3, 1, 2},
                    Player2Characters = new[] {2, 1, 3},
                    StageID = 1,
                };
            }

            return matchForms;
        }
    }
}