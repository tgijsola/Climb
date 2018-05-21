using System.Collections.Generic;

namespace Climb.Models
{
    public class Game
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CharactersPerMatch { get; set; }
        public int MaxMatchPoints { get; set; }

        public HashSet<Character> Characters { get; set; }
        public HashSet<Stage> Stages { get; set; }

        public Game()
        {
        }

        public Game(string name, int charactersPerMatch, int maxMatchPoints)
        {
            Name = name;
            CharactersPerMatch = charactersPerMatch;
            MaxMatchPoints = maxMatchPoints;
        }
    }
}