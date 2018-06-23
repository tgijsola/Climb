using Climb.Data;
using Climb.Models;
using Climb.Services;

namespace Climb.ViewModels.Sets
{
    public class DetailsViewModel
    {
        public Set Set { get; }
        public string P1ProfilePic { get; }
        public string P2ProfilePic { get; }
        public bool UserIsPlaying { get; }
        public string OpponentProfilePic { get; }

        private DetailsViewModel(Set set, string p1ProfilePic, string p2ProfilePic, bool userIsPlaying, string opponentProfilePic)
        {
            Set = set;
            P1ProfilePic = p1ProfilePic;
            P2ProfilePic = p2ProfilePic;
            UserIsPlaying = userIsPlaying;
            OpponentProfilePic = opponentProfilePic;
        }

        public static DetailsViewModel Create(ApplicationUser viewingUser, Set set, ICdnService cdnService)
        {
            var p1ProfilePic = set.Player1.User.GetProfilePicUrl(cdnService);
            var p2ProfilePic = set.Player2.User.GetProfilePicUrl(cdnService);
            var isPlaying = viewingUser != null && (set.Player1.UserID == viewingUser.Id || set.Player2.UserID == viewingUser.Id);
            var opponentProfilePic = set.Player1.UserID == viewingUser?.Id ? p2ProfilePic : p1ProfilePic;

            return new DetailsViewModel(set, p1ProfilePic, p2ProfilePic, isPlaying, opponentProfilePic);
        }
    }
}