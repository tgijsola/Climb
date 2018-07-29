using System.Collections.Generic;

namespace Climb.Core.TieBreakers
{
    public interface ITieBreaker
    {
        ITieBreaker AddAttempt(TieBreakAttempt tieBreakAttempt);
        void Break(IReadOnlyList<Participant> participants);
    }
}
