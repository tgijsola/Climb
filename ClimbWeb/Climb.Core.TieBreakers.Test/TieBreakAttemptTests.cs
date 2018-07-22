using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Climb.Core.TieBreakers.Test
{
    [TestFixture]
    public class TieBreakAttemptTests
    {
        private class FakeTieBreakAttempt : TieBreakAttempt
        {
            public readonly List<Participant> participants = new List<Participant>();

            protected override int GetUserScore(IReadOnlyList<Participant> participants, Participant current)
            {
                this.participants.Add(current);
                return 0;
            }
        }

        private FakeTieBreakAttempt testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new FakeTieBreakAttempt();
        }

        [Test]
        public void Evaluate_AllUsersEvaulated()
        {
            var participants = new[]
            {
                new Participant(0, 0, 0, DateTime.MinValue),
                new Participant(0, 0, 0, DateTime.MinValue),
                new Participant(0, 0, 0, DateTime.MinValue),
            };

            testObj.Evaluate(participants);

            foreach(var participant in participants)
            {
                testObj.participants.Remove(participant);
            }

            Assert.AreEqual(0, testObj.participants.Count);
        }
    }
}