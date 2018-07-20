using System;
using System.Collections.Generic;
using System.Linq;

namespace Climb.Core.TieBreakers.Internal
{
    internal class TieBreaker : ITieBreaker
    {
        private readonly List<TieBreakAttempt> attempts = new List<TieBreakAttempt>();

        public void AddAttempt(TieBreakAttempt tieBreakAttempt)
        {
            if(tieBreakAttempt == null)
            {
                throw new ArgumentNullException(nameof(tieBreakAttempt));
            }

            if(attempts.Any(a => a.GetType() == tieBreakAttempt.GetType()))
            {
                throw new ArgumentException(nameof(tieBreakAttempt));
            }

            attempts.Add(tieBreakAttempt);
        }

        public void Break(IReadOnlyList<Participant> participants)
        {
            var round = attempts.Count;
            foreach(var tiebreakAttempt in attempts)
            {
                var userScores = tiebreakAttempt.Evaluate(participants);
                userScores.Sort();

                var lastScore = int.MinValue;
                var place = 0;
                foreach(var userScore in userScores)
                {
                    if(userScore.RoundPoints != lastScore)
                    {
                        lastScore = userScore.RoundPoints;
                        ++place;
                    }

                    var roundScore = (decimal)Math.Pow(place * 2, round);
                    userScore.Participant.TieBreakerPoints += roundScore;
                }

                --round;
            }
        }
    }
}