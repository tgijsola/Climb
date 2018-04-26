using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.Repositories
{
    public abstract class DbRepository<T> : IDbRepository<T> where T : class
    {
        protected readonly DbSet<T> dbSet;

        protected DbRepository(DbSet<T> dbSet)
        {
            this.dbSet = dbSet;
        }

        public Task<List<T>> ListAll()
        {
            return dbSet.ToListAsync();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }

        public async Task<T> First(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate);
        }
    }
}