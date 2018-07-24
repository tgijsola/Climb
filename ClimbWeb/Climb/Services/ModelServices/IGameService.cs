using System.Threading.Tasks;
using Climb.Models;
using Climb.Requests.Games;

namespace Climb.Services.ModelServices
{
    public interface IGameService
    {
        Task<Game> Create(CreateRequest request);
        Task<Character> AddCharacter(int gameID, string name, string imageKey);
        Task<Stage> AddStage(AddStageRequest request);
    }
}