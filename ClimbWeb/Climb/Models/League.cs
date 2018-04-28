using System.Collections.Generic;
using Newtonsoft.Json;

namespace Climb.Models
{
    public class League
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public Game Game { get; set; }
        [JsonIgnore]
        public HashSet<Season> Seasons { get; set; }

        public League()
        {
        }

        public League(int gameID, string name)
        {
            GameID = gameID;
            Name = name;
        }
    }
}