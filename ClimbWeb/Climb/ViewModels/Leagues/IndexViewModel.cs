using System.Collections.Generic;
using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels.Leagues
{
    public class IndexViewModel : BaseViewModel
    {
        public IReadOnlyList<League> AllLeagues { get; }
        public IReadOnlyList<Game> Games { get; }

        public IndexViewModel(ApplicationUser user, IReadOnlyList<League> allLeagues, IReadOnlyList<Game> games)
            : base(user)
        {
            AllLeagues = allLeagues;
            Games = games;
        }
    }
}