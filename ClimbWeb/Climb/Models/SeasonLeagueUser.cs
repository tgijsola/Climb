using System;
using Newtonsoft.Json;

namespace Climb.Models
{
    public class SeasonLeagueUser : IComparable<SeasonLeagueUser>
    {
        public int SeasonID { get; set; }
        public int LeagueUserID { get; set; }
        public int Standing { get; set; }
        public int Points { get; set; }

        [JsonIgnore]
        public Season Season { get; set; }
        [JsonIgnore]
        public LeagueUser LeagueUser { get; set; }

        public SeasonLeagueUser()
        {
        }

        public SeasonLeagueUser(int seasonID, int leagueUserID)
        {
            SeasonID = seasonID;
            LeagueUserID = leagueUserID;
        }

        public int CompareTo(SeasonLeagueUser other)
        {
            return Points.CompareTo(other.Points);
        }
    }
}