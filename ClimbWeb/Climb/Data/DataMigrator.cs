using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClimbV1.Models;
using Microsoft.EntityFrameworkCore;

namespace Climb.Data
{
    public static class DataMigrator
    {
        private static readonly Dictionary<int, int> gameIDs = new Dictionary<int, int>();

        public static async Task MigrateV1(ApplicationDbContext context)
        {
            await ResetDatabase(context);

            ClimbV1Context v1Context = CreateDB();

            await MigrateUsers(v1Context, context);
            await MigrateGames(v1Context, context);
            await MigrateLeagues(v1Context, context);
        }

        private static async Task ResetDatabase(ApplicationDbContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
        }

        private static ClimbV1Context CreateDB()
        {
            var options = new DbContextOptionsBuilder<ClimbV1Context>()
                            .UseSqlServer("Data Source=climbranks.database.windows.net;Initial Catalog=climbranks;Integrated Security=False;User ID=climbranks_admin;Password=051xu0wvLYM9;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
                            .Options;
            ClimbV1Context context = new ClimbV1Context(options);
            return context;
        }

        private static async Task MigrateUsers(ClimbV1Context v1Context, ApplicationDbContext context)
        {
            var v1Users = await v1Context.User
                .Include(u => u.ApplicationUser).AsNoTracking()
                .ToArrayAsync();
            var users = new ApplicationUser[v1Users.Length];

            for(int i = 0; i < v1Users.Length; i++)
            {
                var v1User = v1Users[i];
                users[i] = new ApplicationUser
                {
                    Id = v1User.ApplicationUser.Id,
                    Email = v1User.ApplicationUser.Email,
                    NormalizedEmail = v1User.ApplicationUser.NormalizedEmail,
                    UserName = v1User.Username,
                    // TODO: Need to normalize.
                    NormalizedUserName = v1User.Username,
                    PasswordHash = v1User.ApplicationUser.PasswordHash,
                    ProfilePicKey = v1User.ProfilePicKey,
                };
            }

            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }

        private static async Task MigrateGames(ClimbV1Context v1Context, ApplicationDbContext context)
        {
            var oldGames = await v1Context.Game.ToArrayAsync();
            var games = new Models.Game[oldGames.Length];

            for(int i = 0; i < oldGames.Length; i++)
            {
                var v1Game = oldGames[i];
                games[i] = new Models.Game
                {
                    Name = v1Game.Name,
                    CharactersPerMatch = v1Game.CharactersPerMatch,
                    MaxMatchPoints = v1Game.MaxMatchPoints,
                    DateAdded = DateTime.Today,
                    HasStages = v1Game.RequireStage,
                };
            }

            context.Games.AddRange(games);
            await context.SaveChangesAsync();

            for(int i = 0; i < oldGames.Length; i++)
            {
                gameIDs[oldGames[i].ID] = games[i].ID;
            }
        }

        private static async Task MigrateLeagues(ClimbV1Context v1Context, ApplicationDbContext context)
        {
            var oldLeagues = await v1Context.League
                .Include(l => l.Admin).ThenInclude(u => u.ApplicationUser).AsNoTracking()
                .ToArrayAsync();
            var leagues = new Models.League[oldLeagues.Length];

            for(int i = 0; i < oldLeagues.Length; i++)
            {
                var oldLeague = oldLeagues[i];
                leagues[i] = new Models.League
                {
                    GameID = gameIDs[oldLeague.GameID],
                    AdminID = oldLeague.Admin.ApplicationUser.Id,
                    Name = oldLeague.Name,
                    DateCreated = DateTime.Today,
                    SetsTillRank = 4,
                };
            }

            context.Leagues.AddRange(leagues);
            await context.SaveChangesAsync();
        }
    }
}
