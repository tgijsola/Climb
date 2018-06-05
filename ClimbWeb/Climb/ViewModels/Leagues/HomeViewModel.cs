using System;
using System.Linq;
using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels.Leagues
{
    public class HomeViewModel : BaseViewModel
    {
        public League League { get; }
        public bool IsMember { get; }

        public HomeViewModel(ApplicationUser user, League league)
            : base(user)
        {
            League = league;

            league.Members.Sort((a, b) => string.Compare(a.User.UserName, b.User.UserName, StringComparison.OrdinalIgnoreCase));
            IsMember = league.Members.Any(lu => lu.UserID == user?.Id);
        }
    }
}