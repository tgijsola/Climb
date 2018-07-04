namespace ClimbV1.Models
{
    public class MatchCharacter
    {
        public int MatchID { get; set; }
        public int CharacterID { get; set; }
        public int LeagueUserID { get; set; }
        public Match Match { get; set; }
        public Character Character { get; set; }
        public LeagueUser LeagueUser { get; set; }
    }
}