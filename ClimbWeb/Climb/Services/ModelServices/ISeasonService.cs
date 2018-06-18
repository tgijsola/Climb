using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Climb.Models;

namespace Climb.Services.ModelServices
{
    public interface ISeasonService
    {
        Task<Season> Create(int leagueID, DateTime start, DateTime end);
        Task<HashSet<Set>> GenerateSchedule(int seasonID);
        Task<Season> UpdateStandings(int setID);
    }
}