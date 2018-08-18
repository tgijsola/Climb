﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ClimbV1.Models
{
    public class ClimbV1Context : IdentityDbContext<ApplicationUser>
    {
        public ClimbV1Context(DbContextOptions<ClimbV1Context> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Character> Character { get; set; }
        public DbSet<Stage> Stage { get; set; }
        public DbSet<League> League { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<Set> Set { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<LeagueUser> LeagueUser { get; set; }
        public DbSet<LeagueUserSeason> LeagueUserSeason { get; set; }
        public DbSet<RankSnapshot> RankSnapshot { get; set; }
        public DbSet<MatchCharacter> MatchCharacters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach(var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<LeagueUserSeason>()
                .HasKey(lus => new { lus.LeagueUserID, lus.SeasonID });

            modelBuilder.Entity<LeagueUserSeason>()
                .HasOne(lus => lus.LeagueUser)
                .WithMany(lu => lu.Seasons)
                .HasForeignKey(lus => lus.LeagueUserID);

            modelBuilder.Entity<LeagueUserSeason>()
                .HasOne(lus => lus.Season)
                .WithMany(s => s.Participants)
                .HasForeignKey(lus => lus.SeasonID);

            modelBuilder.Entity<MatchCharacter>()
                .HasKey(mc => new {mc.MatchID, mc.LeagueUserID, mc.CharacterID});
        }
    }
}