﻿using System;

namespace ClimbV1.Models
{
    public class RankSnapshot
    {
        public int ID { get; set; }
        public int LeagueUserID { get; set; }
        public int Rank { get; set; }
        public int DeltaRank { get; set; }
        public int Points { get; set; }
        public int DeltaPoints { get; set; }
        public DateTime CreatedDate { get; set; }
        public LeagueUser LeagueUser { get; set; }
    }
}
