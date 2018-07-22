using System.Collections.Generic;
using Climb.Core.TieBreakers.Internal;

namespace Climb.Core.TieBreakers
{
    public abstract class TieBreakAttempt
    {
        protected abstract int GetUserScore(IReadOnlyList<Participant> participants, Participant current);

        internal List<TieBreakerScore> Evaluate(IReadOnlyList<Participant> participants)
        {
            var scores = new List<TieBreakerScore>();

            foreach(var data in participants)
            {
                var score = GetUserScore(participants, data);
                scores.Add(new TieBreakerScore(data, score));
            }

            return scores;
        }
    }
}