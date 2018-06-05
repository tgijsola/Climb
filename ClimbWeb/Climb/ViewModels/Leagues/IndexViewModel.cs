using System.Collections.Generic;
using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels.Leagues
{
    public class IndexViewModel : BaseViewModel
    {
        public IReadOnlyList<League> Leagues { get; }
        public IReadOnlyList<Game> Games { get; }

        public IndexViewModel(ApplicationUser user, IReadOnlyList<League> leagues, IReadOnlyList<Game> games)
            : base(user)
        {
            Leagues = leagues;
            Games = games;
        }
    }
}