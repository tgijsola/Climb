namespace Climb.Requests.Leagues
{
    public class JoinRequest
    {
        public int LeagueID { get; set; }
        public string UserID { get; set; }

        public JoinRequest()
        {
        }

        public JoinRequest(int leagueID, string userID)
        {
            LeagueID = leagueID;
            UserID = userID;
        }
    }
}