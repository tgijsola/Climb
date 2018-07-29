using System;
using Climb.Core.TieBreakers.New.Rules;

namespace Climb.Core.TieBreakers.New
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