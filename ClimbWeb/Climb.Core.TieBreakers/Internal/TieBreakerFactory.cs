using Climb.Core.TieBreakers.Internal;

namespace Climb.Core.TieBreakers
{
    public class TieBreakerFactory : ITieBreakerFactory
    {
        public ITieBreaker Create()
        {
            return new TieBreaker()
                .AddAttempt(new TotalWinsTieBreak())
                .AddAttempt(new TiedWinsTieBreak())
                .AddAttempt(new LeaguePointsTieBreak())
                .AddAttempt(new MembershipDurationTieBreak());
        }
    }
}