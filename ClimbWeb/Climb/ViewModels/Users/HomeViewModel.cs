using System.Collections.Generic;
using System.Linq;
using Climb.Data;
using Climb.Models;
using Climb.Services;

namespace Climb.ViewModels.Users
{
    public class HomeViewModel : BaseViewModel
    {
        public ApplicationUser HomeUser { get; }
        public string ProfilePic { get; }
        public bool IsViewingUserHome => User.Id == HomeUser.Id;
        public IReadOnlyList<Set> RecentSets { get; }
        public IReadOnlyList<Set> AvailableSets { get; }

        private HomeViewModel(ApplicationUser user, ApplicationUser homeUser, string profilePic, IReadOnlyList<Set> recentSets, IReadOnlyList<Set> availableSets)
            : base(user)
        {
            HomeUser = homeUser;
            ProfilePic = profilePic;
            RecentSets = recentSets;
            AvailableSets = availableSets;
        }

        public static HomeViewModel Create(ApplicationUser user, ApplicationUser homeUser, ICdnService cdnService)
        {
            var profilePic = homeUser.GetProfilePicUrl(cdnService);
            var sets = homeUser.LeagueUsers.SelectMany(lu => lu.P1Sets.Union(lu.P2Sets)).ToArray();
            var recentSets = sets.Where(s => s.IsComplete).Take(10).ToArray();
            var availableSets = sets.Where(s => !s.IsComplete).Take(10).ToArray();

            return new HomeViewModel(user, homeUser, profilePic, recentSets, availableSets);
        }
    }
}