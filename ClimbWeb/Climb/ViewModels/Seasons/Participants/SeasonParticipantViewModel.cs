using Climb.Models;
using Climb.Services;

namespace Climb.ViewModels.Seasons
{
    public class SeasonParticipantViewModel
    {
        public SeasonLeagueUser Participant { get; }
        public string ProfilePic { get; }

        private SeasonParticipantViewModel(SeasonLeagueUser participant, string profilePic)
        {
            Participant = participant;
            ProfilePic = profilePic;
        }

        public static SeasonParticipantViewModel Create(SeasonLeagueUser participant, ICdnService cdnService)
        {
            var profilePic = cdnService.GetImageUrl(participant.LeagueUser.User.ProfilePicKey, ClimbImageRules.ProfilePic);
            return new SeasonParticipantViewModel(participant, profilePic);
        }
    }
}