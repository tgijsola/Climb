using System.ComponentModel.DataAnnotations;

namespace Climb.Requests.Games
{
    public class AddStageRequest
    {
        [Required]
        public int GameID { get; set; }
        [Required]
        public string Name { get; set; }

        public AddStageRequest()
        {
        }

        public AddStageRequest(int gameID, string name)
        {
            GameID = gameID;
            Name = name;
        }
    }
}