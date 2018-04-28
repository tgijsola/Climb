using System.Threading.Tasks;
using Climb.Models;

namespace Climb.Services.ModelServices
{
    public interface ILeagueService
    {
        Task<League> Create(string name, int gameID);
        Task<LeagueUser> Join(int leagueID, string userID);
    }
}