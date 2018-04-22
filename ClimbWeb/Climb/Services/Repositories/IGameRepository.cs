using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Climb.Models;

namespace Climb.Services.Repositories
{
    public interface IGameRepository
    {
        Task<List<Game>> ListAll();
        Task<bool> AnyExist(string name);
        Task<Game> Create(string name);
        Task<bool> Any(Expression<Func<Game, bool>> predicate);
    }
}