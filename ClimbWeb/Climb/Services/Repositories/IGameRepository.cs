using System.Threading.Tasks;
using Climb.Models;

namespace Climb.Services.Repositories
{
    public interface IGameRepository
    {
        Task<bool> AnyExist(string name);
        Task<Game> Create(string name);
    }
}