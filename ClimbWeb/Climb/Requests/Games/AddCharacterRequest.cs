using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Climb.Requests.Games
{
    public class AddCharacterRequest
    {
        [Required]
        public int GameID { get; set; }
        public int? CharacterID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}