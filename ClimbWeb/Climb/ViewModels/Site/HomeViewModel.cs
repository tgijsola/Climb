using System.Collections.Generic;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using Microsoft.EntityFrameworkCore;

namespace Climb.ViewModels.Site
{
    public class HomeViewModel : BaseViewModel
    {
        public IReadOnlyList<Game> Games { get; }

        private HomeViewModel(ApplicationUser user, IReadOnlyList<Game> games)
            : base(user)
        {
            Games = games;
        }

        public static async Task<HomeViewModel> Create(ApplicationUser user, ApplicationDbContext dbContext)
        {
            var games = await dbContext.Games.ToArrayAsync();

            return new HomeViewModel(user, games);
        }
    }
}