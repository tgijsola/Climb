using System.Collections.Generic;

namespace Climb.Core.TieBreakers.Internal
{
    internal class LeaguePointsTieBreak : TieBreakAttempt
    {
        protected override int GetUserScore(IReadOnlyList<Participant> participants, Participant current)
        {
            return current.LeaguePoints;
        }
    }
}