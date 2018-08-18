﻿using System;
using System.Collections.Generic;
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
                .EnableSensitiveDataLogging()
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

        public static List<T> AddNewRange<T>(ApplicationDbContext dbContext, int count, Action<T, int> preprocess = null) where T : class, new()
        {
            var entries = new List<T>();

            for(var i = 0; i < count; i++)
            {
                var entry = new T();
                preprocess?.Invoke(entry, i);
                entries.Add(entry);
            }

            dbContext.AddRange(entries);
            dbContext.SaveChanges();

            return entries;
        }

        public static void UpdateAndSave<T>(ApplicationDbContext dbContext, T entity, Action update) where T : class
        {
            dbContext.Update(entity);
            update();
            dbContext.SaveChanges();
        }
    }
}