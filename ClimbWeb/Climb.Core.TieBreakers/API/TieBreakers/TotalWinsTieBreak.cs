using System.Collections.Generic;

namespace Climb.Core.TieBreakers.Internal
{
    internal class TotalWinsTieBreak : TieBreakAttempt
    {
        protected override int GetUserScore(IReadOnlyList<Participant> participants, Participant current)
        {
            return current.Wins;
        }
    }
}