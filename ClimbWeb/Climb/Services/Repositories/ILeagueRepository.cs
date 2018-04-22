using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Climb.Models;

namespace Climb.Services.Repositories
{
    public interface ILeagueRepository
    {
        Task<League> Create(string name, int gameID);
        Task<List<League>> ListAll();
        Task<bool> Any(Expression<Func<League, bool>> predicate);
    }
}