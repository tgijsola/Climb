using System.Collections.Generic;

namespace Climb.Core.TieBreakers
{
    public interface ITieBreaker
    {
        void AddAttempt(TieBreakAttempt tieBreakAttempt);
        void Break(IReadOnlyList<Participant> participants);
    }
}
