using System;
using System.Threading.Tasks;
using ClimbV1.Models;
using Microsoft.EntityFrameworkCore;

namespace Climb.Data
{
    public static class DataMigrator
    {
        public static async Task MigrateV1(ApplicationDbContext context)
        {
            await ResetDatabase(context);

            ClimbV1Context v1Context = CreateDB();

            await MigrateUsers(v1Context, context);
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
    }
}
