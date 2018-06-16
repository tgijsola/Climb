using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels.Games
{
    public class StageAddViewModel : BaseViewModel
    {
        public Game Game { get; }

        public StageAddViewModel(ApplicationUser user, Game game)
            : base(user)
        {
            Game = game;
        }
    }
}