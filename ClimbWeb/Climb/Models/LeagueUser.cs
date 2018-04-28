using Climb.Data;

namespace Climb.Models
{
    public class LeagueUser
    {
        public int ID { get; set; }
        public int LeagueID { get; set; }
        public string UserID { get; set; }
        public bool HasLeft { get; set; }

        public League League { get; set; }
        public ApplicationUser User { get; set; }

        public LeagueUser()
        {
        }

        public LeagueUser(int leagueID, string userID)
        {
            LeagueID = leagueID;
            UserID = userID;
        }
    }
}