using System.Threading.Tasks;
using Climb.Models;

namespace Climb.Services.ModelServices
{
    public interface IGameService
    {
        Task<Game> Create(string name);
    }
}