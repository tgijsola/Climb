using System.Collections.Generic;

namespace Climb.Core.TieBreakers.New
{
    public interface ITieBreaker
    {
        void Break(IReadOnlyDictionary<IParticipant, ParticipantRecord> participants);
    }
}