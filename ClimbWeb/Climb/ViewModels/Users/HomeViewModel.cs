using Climb.Data;
using Climb.Services;

namespace Climb.ViewModels.Users
{
    public class HomeViewModel : BaseViewModel
    {
        public string ProfilePic { get; }

        public HomeViewModel(ApplicationUser user, string profilePic)
            : base(user)
        {
            ProfilePic = profilePic;
        }


        public static HomeViewModel Create(ApplicationUser user, ICdnService cdnService)
        {
            var profilePic = cdnService.GetImageUrl(user.ProfilePicKey, ClimbImageRules.ProfilePic);
            return new HomeViewModel(user, profilePic);
        }
    }
}