using System.Collections.Generic;
using System.Linq;
using Climb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Climb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<LeagueUser> LeagueUsers { get; set; }
        public DbSet<SeasonLeagueUser> SeasonLeagueUsers { get; set; }
        public DbSet<Set> Sets { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach(var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            CreateLeagueUser(builder.Entity<LeagueUser>());
            CreateSeasonLeagueUser(builder.Entity<SeasonLeagueUser>());
            CreateSet(builder.Entity<Set>());
        }

        private static void CreateLeagueUser(EntityTypeBuilder<LeagueUser> entity)
        {
            entity.HasQueryFilter(lu => lu.HasLeft == false);
        }

        private static void CreateSeasonLeagueUser(EntityTypeBuilder<SeasonLeagueUser> entity)
        {
            entity.HasKey(lus => new {lus.LeagueUserID, lus.SeasonID});
        }

        private static void CreateSet(EntityTypeBuilder<Set> entity)
        {
        }
    }
}