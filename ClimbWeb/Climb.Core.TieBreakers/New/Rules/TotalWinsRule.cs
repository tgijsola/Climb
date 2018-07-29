using System.Collections.Generic;

namespace Climb.Core.TieBreakers.New.Rules
{
    internal class TotalWinsRule : TieBreakerRule
    {
        protected override int GetParticipantScore((IParticipant participant, ParticipantRecord record) participant, IReadOnlyDictionary<IParticipant, ParticipantRecord> tiedParticipants)
        {
            return participant.record.TotalWins;
        }
    }
}