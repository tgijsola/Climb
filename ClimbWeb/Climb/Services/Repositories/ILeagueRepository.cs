using System.Threading.Tasks;
using Climb.Models;

namespace Climb.Services.Repositories
{
    public interface ILeagueRepository : IDbRepository<League>
    {
        Task<League> Create(string name, int gameID);
    }
}