using Climb.Models;

namespace Climb.Responses.Models
{
    public class LeagueUserDto
    {
        public readonly int id;
        public readonly int leagueID;
        public readonly string userID;
        public readonly bool hasLeft;
        public readonly string username;

        public LeagueUserDto(LeagueUser leagueUser)
        {
            id = leagueUser.ID;
            leagueID = leagueUser.ID;
            userID = leagueUser.UserID;
            hasLeft = leagueUser.HasLeft;
            username = leagueUser.User.UserName;
        }
    }
}