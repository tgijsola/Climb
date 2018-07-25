using System;
using System.Collections.Generic;

namespace ClimbV1.Models
{
    public class Season
    {
        public int ID { get; set; }
        public int LeagueID { get; set; }
        public int Index { get; set; }
        public DateTime StartDate { get; set; }
        public int ChallongeID { get; set; }
        public string ChallongeUrl { get; set; }
        public bool IsComplete { get; set; }
        public League League { get; set; }
        public HashSet<LeagueUserSeason> Participants { get; set; }
        public HashSet<Set> Sets { get; set; }
    }
}
