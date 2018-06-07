using System.Collections.Generic;
using System.Linq;
using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels.Leagues
{
    public class HomeViewModel : BaseViewModel
    {
        public League League { get; }
        public bool IsMember { get; }
        public IReadOnlyList<LeagueUser> Members { get; }

        public HomeViewModel(ApplicationUser user, League league)
            : base(user)
        {
            League = league;

            league.Members.Sort();
            Members = league.Members;
            IsMember = league.Members.Any(lu => lu.UserID == user?.Id);
        }
    }
}