using System.Collections.Generic;
using Climb.Requests.Sets;

namespace Climb.Services.ModelServices
{
    public interface ISetService
    {
        void Update(int setID, List<MatchForm> matches);
    }
}