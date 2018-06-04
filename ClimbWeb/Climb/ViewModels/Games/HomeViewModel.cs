using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels.Games
{
    public class HomeViewModel : BaseViewModel
    {
        public Game Game { get; }

        public HomeViewModel(ApplicationUser user, Game game)
            : base(user)
        {
            Game = game;
        }
    }
}