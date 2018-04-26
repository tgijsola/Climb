using System.Threading.Tasks;
using Climb.Models;

namespace Climb.Services.Repositories
{
    public interface ILeagueRepository
    {
        Task<League> Create(string name, int gameID);
    }
}