using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Climb.Models
{
    public class SeasonLeagueUser : IComparable<SeasonLeagueUser>
    {
        public int ID { get; set; }
        public int SeasonID { get; set; }
        public int LeagueUserID { get; set; }
        public int Standing { get; set; }
        public int Points { get; set; }

        [JsonIgnore]
        public Season Season { get; set; }
        [JsonIgnore]
        public LeagueUser LeagueUser { get; set; }
        [JsonIgnore]
        public List<Set> P1Sets { get; set; }
        [JsonIgnore]
        public List<Set> P2Sets { get; set; }

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