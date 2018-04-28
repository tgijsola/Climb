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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            CreateLeagueUser(builder.Entity<LeagueUser>());
            CreateSeasonLeagueUser(builder.Entity<SeasonLeagueUser>());
        }

        private static void CreateLeagueUser(EntityTypeBuilder<LeagueUser> entity)
        {
            entity.HasQueryFilter(lu => lu.HasLeft == false);
        }

        private static void CreateSeasonLeagueUser(EntityTypeBuilder<SeasonLeagueUser> entity)
        {
            entity.HasKey(lus => new {lus.LeagueUserID, lus.SeasonID});

            entity
                .HasOne(lus => lus.Season)
                .WithMany(l => l.Participants)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}