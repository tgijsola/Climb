using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Climb.Models
{
    public class Set
    {
        public int ID { get; set; }
        public int LeagueID { get; set; }
        public int? SeasonID { get; set; }
        public int Player1ID { get; set; }
        public int Player2ID { get; set; }
        public int? Player1Score { get; set; }
        public int? Player2Score { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [JsonIgnore]
        public League League { get; set; }
        [JsonIgnore]
        public Season Season { get; set; }
        [JsonIgnore]
        [InverseProperty("P1Sets")]
        [ForeignKey("Player1ID")]
        public LeagueUser Player1 { get; set; }
        [JsonIgnore]
        [InverseProperty("P2Sets")]
        [ForeignKey("Player2ID")]
        public LeagueUser Player2 { get; set; }
        [Required]
        public List<Match> Matches { get; set; }

        public Set()
        {
        }

        public Set(int leagueID, int seasonID, int player1ID, int player2ID, DateTime dueDate)
        {
            LeagueID = leagueID;
            SeasonID = seasonID;
            Player1ID = player1ID;
            Player2ID = player2ID;
            DueDate = dueDate;
        }

        public bool IsPlaying(int leagueUserID)
        {
            return Player1ID == leagueUserID || Player2ID == leagueUserID;
        }

        public int GetOpponentID(int leagueUserID)
        {
            if(Player1ID == leagueUserID)
            {
                return Player2ID;
            }

            if(Player2ID == leagueUserID)
            {
                return Player1ID;
            }

            throw new ArgumentException($"LeagueUser with ID '{leagueUserID}' is not playing this set.");
        }
    }
}