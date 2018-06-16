using Climb.Models;
using Climb.Services;

namespace Climb.ViewModels.Leagues
{
    public class LeagueUserViewModel
    {
        public LeagueUser LeagueUser { get; }
        public string ProfilePic { get; }

        private LeagueUserViewModel(LeagueUser leagueUser, string profilePic)
        {
            LeagueUser = leagueUser;
            ProfilePic = profilePic;
        }

        public static LeagueUserViewModel Create(LeagueUser leagueUser, ICdnService cdnService)
        {
            var profilePic = cdnService.GetImageUrl(leagueUser.User.ProfilePicKey, ClimbImageRules.ProfilePic);
            return new LeagueUserViewModel(leagueUser, profilePic);
        }
    }
}