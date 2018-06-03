using System.Collections.Generic;
using System.Linq;
using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels
{
    public abstract class BaseViewModel
    {
        public ApplicationUser User { get; }
        public IReadOnlyList<League> Leagues { get; }
        //public IReadOnlyList<Season> Seasons { get; }

        protected BaseViewModel(ApplicationUser user)
        {
            User = user;

            var leagues = user?.LeagueUsers
                .Select(lu => lu.League)
                .OrderBy(l => l.Name)
                .ToArray();
            Leagues = leagues;

            //var seasons = user.LeagueUsers
            //    .Select(lu => lu.Seasons.FirstOrDefault(slu => slu.Season.st))
        }
    }
}