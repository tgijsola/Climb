using System.Linq;
using Climb.Models;
using Climb.Services;

namespace Climb.Responses.Models
{
    public class GameDto
    {
        public int ID { get; }
        public string Name { get; }
        public CharacterDto[] Characters { get; }
        public StageDto[] Stages { get; }

        private GameDto(int id, string name, CharacterDto[] characters, StageDto[] stages)
        {
            ID = id;
            Name = name;
            Characters = characters;
            Stages = stages;
        }

        public static GameDto Create(Game game, ICdnService cdnService)
        {
            var characters = game.Characters.Select(c => CharacterDto.Create(c, cdnService)).ToArray();
            var stages = game.Stages.Select(s => new StageDto(s.ID, s.Name)).ToArray();

            return new GameDto(game.ID, game.Name, characters, stages);
        }
    }
}