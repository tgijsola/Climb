using System.Collections.Generic;
using Newtonsoft.Json;

namespace Climb.Models
{
    public class Match
    {
        public int SetID { get; set; }
        public int Index { get; set; }
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
        public int? StageID { get; set; }

        [JsonIgnore]
        public Set Set { get; set; }
        [JsonIgnore]
        public Stage Stage { get; set; }
        public HashSet<MatchCharacter> MatchCharacters { get; set; }
    }
}