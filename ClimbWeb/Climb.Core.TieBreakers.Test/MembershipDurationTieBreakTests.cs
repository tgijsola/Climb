using System;
using Climb.Core.TieBreakers.Internal;
using NUnit.Framework;

namespace Climb.Core.TieBreakers.Test
{
    [TestFixture]
    public class MembershipDurationTieBreakTests
    {
        private MembershipDurationTieBreak testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new MembershipDurationTieBreak();
        }

        [Test]
        public void Evaluate_ReturnMembershipSeconds()
        {
            var participants = new[]
            {
                AddParticipant(1, 3),
                AddParticipant(2, 4),
                AddParticipant(3, 5),
            };

            var results = testObj.Evaluate(participants);

            Assert.AreEqual(3*7*24*60*60, results[0].RoundPoints);
            Assert.AreEqual(4*7*24*60*60, results[1].RoundPoints);
            Assert.AreEqual(5*7*24*60*60, results[2].RoundPoints);
        }

        private static Participant AddParticipant(int id, int weeks)
        {
            return new Participant(id, 0, 0, DateTime.Today.AddDays(-weeks * 7));
        }
    }
}