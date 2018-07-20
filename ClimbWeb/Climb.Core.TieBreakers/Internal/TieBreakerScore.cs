using System;

namespace Climb.Core.TieBreakers.Internal
{
    internal class TieBreakerScore : IComparable<TieBreakerScore>
    {
        public Participant Participant { get; }
        public int RoundPoints { get; }

        public TieBreakerScore(Participant participant, int roundPoints)
        {
            Participant = participant;
            RoundPoints = roundPoints;
        }

        public int CompareTo(TieBreakerScore other) => RoundPoints.CompareTo(other.RoundPoints);
    }
}