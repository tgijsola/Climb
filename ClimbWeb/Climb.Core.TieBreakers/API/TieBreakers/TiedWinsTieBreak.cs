using System.Collections.Generic;
using System.Linq;

namespace Climb.Core.TieBreakers.Internal
{
    internal class TiedWinsTieBreak : TieBreakAttempt
    {
        protected override int GetUserScore(IReadOnlyList<Participant> participants, Participant current)
        {
            return participants.Sum(p => current.TimesBeatenOpponent(p.UserID));
        }
    }
}