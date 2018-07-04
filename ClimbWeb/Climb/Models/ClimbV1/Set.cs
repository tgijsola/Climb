using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimbV1.Models
{
    public class Set
    {
        public int ID { get; set; }
        public int? SeasonID { get; set; }
        public int LeagueID { get; set; }
        public int? Player1ID { get; set; }
        public int? Player2ID { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Player1Score { get; set; }
        public int? Player2Score { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeactivated { get; set; }

        [InverseProperty("P1Sets")]
        [ForeignKey("Player1ID")]
        public LeagueUser Player1 { get; set; }
        [InverseProperty("P2Sets")]
        [ForeignKey("Player2ID")]
        public LeagueUser Player2 { get; set; }
        public ICollection<Match> Matches { get; set; }
        public Season Season { get; set; }
        public League League { get; set; }
    }
}
