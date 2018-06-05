using System.Collections.Generic;
using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels.Games
{
    public class IndexViewModel : BaseViewModel
    {
        public IReadOnlyList<Game> Games { get; }

        public IndexViewModel(ApplicationUser user, IReadOnlyList<Game> games)
            : base(user)
        {
            Games = games;
        }
    }
}