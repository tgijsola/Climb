using System.Collections.Generic;
using Climb.Core.TieBreakers.Internal;

namespace Climb.Core.TieBreakers
{
    public abstract class TieBreakAttempt
    {
        protected abstract int GetUserScore(IReadOnlyList<Participant> participants, Participant current);

        internal List<TieBreakerScore> Evaluate(IReadOnlyList<Participant> standingData)
        {
            var scores = new List<TieBreakerScore>();

            foreach(var data in standingData)
            {
                var score = GetUserScore(standingData, data);
                scores.Add(new TieBreakerScore(data, score));
            }

            return scores;
        }
    }
}