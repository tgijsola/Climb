using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Climb.Models
{
    public class Game
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; } = "";
        public int CharactersPerMatch { get; set; }
        public int MaxMatchPoints { get; set; }
        public DateTime DateAdded { get; set; }

        [Required]
        public List<Character> Characters { get; set; }
        [Required]
        public List<Stage> Stages { get; set; }
        public List<League> Leagues { get; set; }

        public Game()
        {
        }

        public Game(string name, int charactersPerMatch, int maxMatchPoints)
        {
            Name = name;
            CharactersPerMatch = charactersPerMatch;
            MaxMatchPoints = maxMatchPoints;
            DateAdded = DateTime.Today;
        }
    }
}