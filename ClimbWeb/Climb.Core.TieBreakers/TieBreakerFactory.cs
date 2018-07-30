using System;

namespace Climb.Core.TieBreakers
{
    public class TieBreakerFactory : ITieBreakerFactory
    {
        public ITieBreaker Create()
        {
            return new TieBreaker()
                .AddRule(new TotalWinsRule())
                .AddRule(new TiedWinsRule())
                .AddRule(new LeaguePointsRule())
                .AddRule(new MembershipDurationRule(DateTime.Now));
        }
    }
}