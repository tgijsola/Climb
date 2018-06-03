using Climb.Data;
using Climb.Services;

namespace Climb.ViewModels.Users
{
    public class HomeViewModel : BaseViewModel
    {
        public ApplicationUser HomeUser { get; }
        public string ProfilePic { get; }
        public bool IsViewingUserHome => User.Id == HomeUser.Id;

        private HomeViewModel(ApplicationUser user, ApplicationUser homeUser, string profilePic)
            : base(user)
        {
            HomeUser = homeUser;
            ProfilePic = profilePic;
        }

        public static HomeViewModel Create(ApplicationUser user, ApplicationUser homeUser, ICdnService cdnService)
        {
            var profilePic = cdnService.GetImageUrl(user.ProfilePicKey, ClimbImageRules.ProfilePic);
            return new HomeViewModel(user, homeUser, profilePic);
        }
    }
}