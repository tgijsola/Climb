using System.Collections.Generic;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;

namespace Climb.Services
{
    public interface IScheduleFactory
    {
        Task<HashSet<Set>> GenerateScheduleAsync(Season season, ApplicationDbContext dbContext);
    }
}