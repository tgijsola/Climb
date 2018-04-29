﻿using System.Collections.Generic;
using Climb.Data;
using Newtonsoft.Json;

namespace Climb.Models
{
    public class LeagueUser
    {
        public int ID { get; set; }
        public int LeagueID { get; set; }
        public string UserID { get; set; }
        public bool HasLeft { get; set; }

        [JsonIgnore]
        public League League { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        [JsonIgnore]
        public HashSet<SeasonLeagueUser> Seasons { get; set; }

        #region For DB
        [JsonIgnore]
        public HashSet<Set> P1Sets { get; set; }
        [JsonIgnore]
        public HashSet<Set> P2Sets { get; set; }
        #endregion

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