using System;
using Climb.Core.TieBreakers.Internal;
using NUnit.Framework;

namespace Climb.Core.TieBreakers.Test
{
    [TestFixture]
    public class TiedWinsTieBreakTests
    {
        private TiedWinsTieBreak testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new TiedWinsTieBreak();
        }

        [Test]
        public void Evaluate_ReturnTiedWins()
        {
            var participants = new[]
            {
                AddParticipant(1),
                AddParticipant(2, 1, 3),
                AddParticipant(3, 1),
            };

            var results = testObj.Evaluate(participants);

            Assert.AreEqual(0, results[0].RoundPoints);
            Assert.AreEqual(2, results[1].RoundPoints);
            Assert.AreEqual(1, results[2].RoundPoints);
        }

        private static Participant AddParticipant(int id, params int[] beatenOpponents)
        {
            var participant = new Participant(id, 0, 0, DateTime.MinValue);
            foreach(var opponent in beatenOpponents)
            {
                participant.AddWin(opponent);
            }

            return participant;
        }
    }
}