using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using Climb.Services;
using Microsoft.EntityFrameworkCore;

namespace Climb.ViewModels.Users
{
    public class HomeViewModel : BaseViewModel
    {
        public ApplicationUser HomeUser { get; }
        public string ProfilePic { get; }
        public bool IsViewingUserHome => User.Id == HomeUser.Id;
        public IReadOnlyList<Set> RecentSets { get; }
        public IReadOnlyList<Set> AvailableSets { get; }
        public IReadOnlyList<LeagueUser> SharedLeagues { get; }
        public IReadOnlyList<SetRequest> SetRequests { get; }
        public bool ShowSetRequests { get; }

        private HomeViewModel(ApplicationUser user, ApplicationUser homeUser, string profilePic, IReadOnlyList<Set> recentSets, IReadOnlyList<Set> availableSets, IReadOnlyList<SetRequest> setRequests, bool showSetRequests)
            : base(user)
        {
            HomeUser = homeUser;
            ProfilePic = profilePic;
            RecentSets = recentSets;
            AvailableSets = availableSets;
            SetRequests = setRequests;
            ShowSetRequests = showSetRequests;

            SharedLeagues = homeUser.LeagueUsers.Where(lu => user.LeagueUsers.Any(lu2 => lu2.LeagueID == lu.LeagueID)).ToArray();
        }

        public static async Task<HomeViewModel> CreateAsync(ApplicationUser user, ApplicationUser homeUser, ICdnService cdnService, ApplicationDbContext dbContext)
        {
            var profilePic = homeUser.GetProfilePicUrl(cdnService);
            var sets = homeUser.LeagueUsers.SelectMany(lu => lu.P1Sets.Union(lu.P2Sets)).ToArray();
            var recentSets = sets.Where(s => s.IsComplete).Take(10).ToArray();
            var availableSets = sets.Where(s => !s.IsComplete).Take(10).ToArray();

#if DEBUG
            const bool showSetRequests = true;
#else
            var showSetRequests = user.Id == homeUser.Id;
#endif 

            IReadOnlyList<SetRequest> setRequests = null;
            if(showSetRequests)
            {
                setRequests = await dbContext.SetRequests
                    .Where(sr => user.LeagueUsers.Any(lu => lu.ID == sr.ChallengedID))
                    .OrderByDescending(sr => sr.DateCreated)
                    .ToArrayAsync();
            }

            return new HomeViewModel(user, homeUser, profilePic, recentSets, availableSets, setRequests, showSetRequests);
        }
    }
}