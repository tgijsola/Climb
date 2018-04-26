using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Climb.Services.Repositories
{
    public interface IDbRepository<T> where T : class
    {
        Task<List<T>> ListAll();
        Task<bool> Any(Expression<Func<T, bool>> predicate);
        Task<T> First(Expression<Func<T, bool>> predicate);
    }
}