using System;
using System.Collections.Generic;

namespace Climb.Core.TieBreakers.Internal
{
    internal class MembershipDurationTieBreak : TieBreakAttempt
    {
        protected override int GetUserScore(IReadOnlyList<Participant> participants, Participant current)
        {
            return (int)(DateTime.Today - current.JoinDate).TotalSeconds;
        }
    }
}