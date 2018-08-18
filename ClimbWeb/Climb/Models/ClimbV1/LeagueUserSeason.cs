﻿namespace ClimbV1.Models
{
    public class LeagueUserSeason
    {
        public int LeagueUserID { get; set; }
        public int SeasonID { get; set; }
        public int Standing { get; set; }
        public int Points { get; set; }
        public int PotentialMaxPoints { get; set; }
        public int ChallongeID { get; set; }
        public bool HasLeft { get; set; }
        public LeagueUser LeagueUser { get; set; }
        public Season Season { get; set; }
    }
}