using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels.Games
{
    public class CharacterAddViewModel : BaseViewModel
    {
        public Game Game { get; }

        public CharacterAddViewModel(ApplicationUser user, Game game)
            : base(user)
        {
            Game = game;
        }
    }
}