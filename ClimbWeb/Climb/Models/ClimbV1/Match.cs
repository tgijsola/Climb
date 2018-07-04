using System.Collections.Generic;

namespace ClimbV1.Models
{
    public class Match
    {
        public int ID { get; set; }
        public int Index { get; set; }
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
        public int? StageID { get; set; }
        public Set Set { get; set; }
        public Stage Stage { get; set; }
        public List<MatchCharacter> MatchCharacters { get; set; }
    }
}