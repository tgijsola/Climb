using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Climb.Models
{
    public class League
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        [Required]
        public string Name { get; set; } = "";
        public int SetsTillRank { get; set; } = 4;

        [JsonIgnore]
        public Game Game { get; set; }
        [JsonIgnore]
        public List<LeagueUser> Members { get; set; }
        [JsonIgnore]
        public HashSet<Season> Seasons { get; set; }
        [JsonIgnore]
        public List<Set> Sets { get; set; }

        public League()
        {
        }

        public League(int gameID, string name)
        {
            GameID = gameID;
            Name = name;
        }

        public bool IsMemberNew(LeagueUser member) => member.SetCount >= SetsTillRank;
    }
}