using Climb.Core.TieBreakers.Internal;
using NUnit.Framework;

namespace Climb.Core.TieBreakers.Test
{
    [TestFixture]
    public class LeaguePointsTieBreakTests
    {
        private LeaguePointsTieBreak testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new LeaguePointsTieBreak();
        }

        [Test]
        public void Evaluate_ReturnLeaguePointTotals()
        {
            var participants = new[] {AddParticipant(1, 10), AddParticipant(2, 20), AddParticipant(3, 30)};

            var results = testObj.Evaluate(participants);

            Assert.AreEqual(10, results[0].RoundPoints);
            Assert.AreEqual(20, results[1].RoundPoints);
            Assert.AreEqual(30, results[2].RoundPoints);
        }

        private static Participant AddParticipant(int id, int leaguePoints)
        {
            return new Participant(id, leaguePoints, 0);
        }
    }
}