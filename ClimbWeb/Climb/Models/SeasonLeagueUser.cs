using Newtonsoft.Json;

namespace Climb.Models
{
    public class SeasonLeagueUser
    {
        public int SeasonID { get; set; }
        public int LeagueUserID { get; set; }

        [JsonIgnore]
        public Season Season { get; set; }
        [JsonIgnore]
        public League League { get; set; }

        public SeasonLeagueUser()
        {
        }

        public SeasonLeagueUser(int seasonID, int leagueUserID)
        {
            SeasonID = seasonID;
            LeagueUserID = leagueUserID;
        }
    }
}