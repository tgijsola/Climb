using System;
using System.Threading.Tasks;
using Climb.Models;

namespace Climb.Services.ModelServices
{
    public interface ISeasonService
    {
        Task<Season> Create(int leagueID, DateTime start, DateTime end);
    }
}