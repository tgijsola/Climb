using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Climb.Models
{
    public class Match
    {
        public int ID { get; set; }
        public int SetID { get; set; }
        public int Index { get; set; }
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
        public int? StageID { get; set; }

        [JsonIgnore]
        public Set Set { get; set; }
        [JsonIgnore]
        public Stage Stage { get; set; }
        [Required]
        public HashSet<MatchCharacter> MatchCharacters { get; set; }

        public Match()
        {
        }

        public Match(int setID, int index, int player1Score, int player2Score, int? stageID = null)
        {
            SetID = setID;
            Index = index;
            Player1Score = player1Score;
            Player2Score = player2Score;
            StageID = stageID;
            MatchCharacters = new HashSet<MatchCharacter>();
        }
    }
}