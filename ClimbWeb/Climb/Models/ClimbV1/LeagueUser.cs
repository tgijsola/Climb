using System.Collections.Generic;

namespace ClimbV1.Models
{
    public class LeagueUser
    {
        public enum Trend
        {
            BigUp,
            SmallUp,
            None,
            SmallDown,
            BigDown,
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public int LeagueID { get; set; }
        public string DisplayName { get; set; }
        public int Points { get; set; }
        public string ProfilePicKey { get; set; }
        public bool HasLeft { get; set; }
        public int Rank { get; set; }
        public string SlackUsername { get; set; }
        public string ChallongeUsername { get; set; }
        public int SetsPlayed { get; set; }
        public User User { get; set; }
        public League League { get; set; }
        public HashSet<LeagueUserSeason> Seasons { get; set; }
        public HashSet<Set> P1Sets { get; set; }
        public HashSet<Set> P2Sets { get; set; }
        public HashSet<RankSnapshot> RankSnapshots { get; set; }
    }
}
