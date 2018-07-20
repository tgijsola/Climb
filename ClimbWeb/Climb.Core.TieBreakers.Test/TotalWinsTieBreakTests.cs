using System.Collections.Generic;
using Climb.Core.TieBreakers.Internal;
using NUnit.Framework;

namespace Climb.Core.TieBreakers.Test
{
    [TestFixture]
    public class TotalWinsTieBreakTests
    {
        private TotalWinsTieBreak testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new TotalWinsTieBreak();
        }

        [Test]
        public void GetUserScore_ReturnWinCounts()
        {
            var participants = new List<Participant> {AddParticipant(1, 3), AddParticipant(2, 5), AddParticipant(3, 4)};

            var results = testObj.Evaluate(participants);

            Assert.AreEqual(3, results[0].RoundPoints);
            Assert.AreEqual(5, results[1].RoundPoints);
            Assert.AreEqual(4, results[2].RoundPoints);
        }

        private static Participant AddParticipant(int id, int winCount)
        {
            var participant = new Participant(id, 0, 0);
            for(var i = 0; i < winCount; i++)
            {
                participant.AddWin(0);
            }

            return participant;
        }
    }
}