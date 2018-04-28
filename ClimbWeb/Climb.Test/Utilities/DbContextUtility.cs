using System;
using Climb.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Climb.Test.Utilities
{
    internal static class DbContextUtility
    {
        public static ApplicationDbContext CreateMockDb()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        public static T AddNew<T>(ApplicationDbContext dbContext, Action<T> preprocess = null) where T : class, new()
        {
            var entry = new T();
            preprocess?.Invoke(entry);
            var model = dbContext.Add(entry);
            dbContext.SaveChanges();
            return model.Entity;
        }
    }
}