using System.ComponentModel.DataAnnotations;

namespace Climb.Requests.Games
{
    public class AddCharacterRequest
    {
        [Required]
        public int GameID { get; set; }
        [Required]
        public string Name { get; set; }

        public AddCharacterRequest()
        {
        }

        public AddCharacterRequest(int gameID, string name)
        {
            GameID = gameID;
            Name = name;
        }
    }
}