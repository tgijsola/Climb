using System.Threading.Tasks;
using Climb.Models;
using Climb.Requests.Games;
using Microsoft.AspNetCore.Http;

namespace Climb.Services.ModelServices
{
    public interface IGameService
    {
        Task<Game> Create(CreateRequest request);
        Task<Character> AddCharacter(int gameID, int? characterID, string name, IFormFile imageFile);
        Task<Stage> AddStage(AddStageRequest request);
    }
}