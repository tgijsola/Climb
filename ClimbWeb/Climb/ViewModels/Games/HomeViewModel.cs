using System;
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

            game.Characters.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
            game.Stages.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}