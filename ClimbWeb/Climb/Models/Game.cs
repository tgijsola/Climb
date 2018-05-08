using System.Collections.Generic;

namespace Climb.Models
{
    public class Game
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public HashSet<GameCharacter> Characters { get; set; }
        public HashSet<Stage> Stages { get; set; }

        public Game()
        {
        }

        public Game(string name)
        {
            Name = name;
        }
    }
}