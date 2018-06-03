using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels.Leagues
{
    public class HomeViewModel : BaseViewModel
    {
        public League League { get; }

        public HomeViewModel(ApplicationUser user, League league)
            : base(user)
        {
            League = league;
        }
    }
}