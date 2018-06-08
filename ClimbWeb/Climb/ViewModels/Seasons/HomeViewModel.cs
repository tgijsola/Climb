using System.Collections.Generic;
using System.Linq;
using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels.Seasons
{
    public class HomeViewModel : BaseViewModel
    {
        public Season Season { get; }
        public int SeasonNumber { get; }
        public bool IsParticipant { get; }
        public bool CanStartSeason { get; }
        
        public IReadOnlyList<SeasonLeagueUser> Participants => Season.Participants;

        private HomeViewModel(ApplicationUser user, Season season, bool isParticipant, bool canStartSeason)
            : base(user)
        {
            Season = season;
            IsParticipant = isParticipant;
            CanStartSeason = canStartSeason;
            SeasonNumber = season.Index + 1;

            Season.Participants.Sort();
        }

        public static HomeViewModel Create(ApplicationUser user, Season season)
        {
            var isParticipant = user.LeagueUsers.Any(lu => season.Participants.Any(slu => slu.LeagueUserID == lu.ID));
            return new HomeViewModel(user, season, isParticipant, false);
        }
    }
}