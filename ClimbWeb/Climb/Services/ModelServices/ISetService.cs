using System.Collections.Generic;
using System.Threading.Tasks;
using Climb.Requests.Sets;

namespace Climb.Services.ModelServices
{
    public interface ISetService
    {
        Task Update(int setID, IReadOnlyList<MatchForm> matchForms);
    }
}