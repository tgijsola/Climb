using System;
using System.Collections.Generic;
using Climb.Core.TieBreakers.Internal;
using NUnit.Framework;

namespace Climb.Core.TieBreakers.Test
{
    [TestFixture]
    public class TieBreakerTests
    {
        private class FakeAttempt : TieBreakAttempt
        {
            private readonly int[] scores;
            private int index = -1;

            public FakeAttempt(params int[] scores)
            {
                this.scores = scores;
            }

            protected override int GetUserScore(IReadOnlyList<Participant> participants, Participant current)
            {
                ++index;
                return scores[index];
            }
        }

        private class Fake2Attempt : FakeAttempt
        {
            public Fake2Attempt(params int[] scores)
                : base(scores)
            {
            }
        }

        private class Fake3Attempt : FakeAttempt
        {
            public Fake3Attempt(params int[] scores)
                : base(scores)
            {
            }
        }

        private TieBreaker testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new TieBreaker();
        }

        [Test]
        public void AddAttempt_Null_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => testObj.AddAttempt(null));
        }

        [Test]
        public void AddAttempt_Duplicate_ArgumentException()
        {
            var attempt = new FakeAttempt();
            testObj.AddAttempt(attempt);
            var attempt2 = new FakeAttempt();

            Assert.Throws<ArgumentException>(() => testObj.AddAttempt(attempt2));
        }

        [Test]
        public void Break_1Attempt_ParticipantTieBreakerScoreUpdated()
        {
            // 3rd, 1st, 2nd
            var scores = new[] {2, 14, 7};
            testObj.AddAttempt(new FakeAttempt(scores));
            var participants = CreateParticipants();

            testObj.Break(participants);

            participants.Sort();
            Assert.AreEqual(2, participants[0].UserID);
            Assert.AreEqual(3, participants[1].UserID);
            Assert.AreEqual(1, participants[2].UserID);
        }

        [Test]
        public void Break_MultipleAttempts_FirstAttemptWins()
        {
            // 3rd, 1st, 2nd
            var scores = new[] {2, 14, 7};
            testObj.AddAttempt(new FakeAttempt(scores));
            scores = new[] {200, 14, 7};
            testObj.AddAttempt(new Fake2Attempt(scores));
            scores = new[] {200, 14, 7};
            testObj.AddAttempt(new Fake3Attempt(scores));

            var participants = CreateParticipants();

            testObj.Break(participants);

            participants.Sort();
            Assert.AreEqual(2, participants[0].UserID);
            Assert.AreEqual(3, participants[1].UserID);
            Assert.AreEqual(1, participants[2].UserID);
        }

        private static List<Participant> CreateParticipants()
        {
            return new List<Participant>
            {
                new Participant(1, 0, 0, DateTime.MinValue),
                new Participant(2, 0, 0, DateTime.MinValue),
                new Participant(3, 0, 0, DateTime.MinValue),
            };
        }
    }
}