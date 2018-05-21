﻿// <auto-generated />
using Climb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Climb.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180520060648_AddGameRules")]
    partial class AddGameRules
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Climb.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Climb.Models.Character", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GameID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("GameID");

                    b.ToTable("Character");
                });

            modelBuilder.Entity("Climb.Models.Game", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CharactersPerMatch");

                    b.Property<int>("MaxMatchPoints");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Climb.Models.League", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("GameID");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("Climb.Models.LeagueUser", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("HasLeft");

                    b.Property<int>("LeagueID");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("LeagueID");

                    b.HasIndex("UserID");

                    b.ToTable("LeagueUsers");
                });

            modelBuilder.Entity("Climb.Models.Match", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Index");

                    b.Property<int>("Player1Score");

                    b.Property<int>("Player2Score");

                    b.Property<int>("SetID");

                    b.Property<int?>("StageID");

                    b.HasKey("ID");

                    b.HasIndex("SetID");

                    b.HasIndex("StageID");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Climb.Models.MatchCharacter", b =>
                {
                    b.Property<int>("MatchID");

                    b.Property<int>("CharacterID");

                    b.Property<int>("LeagueUserID");

                    b.HasKey("MatchID", "CharacterID", "LeagueUserID");

                    b.HasIndex("CharacterID");

                    b.HasIndex("LeagueUserID");

                    b.ToTable("MatchCharacters");
                });

            modelBuilder.Entity("Climb.Models.Season", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("Index");

                    b.Property<int>("LeagueID");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("ID");

                    b.HasIndex("LeagueID");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("Climb.Models.SeasonLeagueUser", b =>
                {
                    b.Property<int>("LeagueUserID");

                    b.Property<int>("SeasonID");

                    b.HasKey("LeagueUserID", "SeasonID");

                    b.HasIndex("SeasonID");

                    b.ToTable("SeasonLeagueUsers");
                });

            modelBuilder.Entity("Climb.Models.Set", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DueDate");

                    b.Property<int>("LeagueID");

                    b.Property<int>("Player1ID");

                    b.Property<int?>("Player1Score");

                    b.Property<int>("Player2ID");

                    b.Property<int?>("Player2Score");

                    b.Property<int?>("SeasonID");

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("ID");

                    b.HasIndex("LeagueID");

                    b.HasIndex("Player1ID");

                    b.HasIndex("Player2ID");

                    b.HasIndex("SeasonID");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("Climb.Models.Stage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GameID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("GameID");

                    b.ToTable("Stage");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Climb.Models.Character", b =>
                {
                    b.HasOne("Climb.Models.Game")
                        .WithMany("Characters")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Climb.Models.League", b =>
                {
                    b.HasOne("Climb.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Climb.Models.LeagueUser", b =>
                {
                    b.HasOne("Climb.Models.League", "League")
                        .WithMany("Members")
                        .HasForeignKey("LeagueID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Climb.Data.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Climb.Models.Match", b =>
                {
                    b.HasOne("Climb.Models.Set", "Set")
                        .WithMany("Matches")
                        .HasForeignKey("SetID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Climb.Models.Stage", "Stage")
                        .WithMany()
                        .HasForeignKey("StageID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Climb.Models.MatchCharacter", b =>
                {
                    b.HasOne("Climb.Models.Character", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Climb.Models.LeagueUser", "LeagueUser")
                        .WithMany()
                        .HasForeignKey("LeagueUserID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Climb.Models.Match", "Match")
                        .WithMany("MatchCharacters")
                        .HasForeignKey("MatchID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Climb.Models.Season", b =>
                {
                    b.HasOne("Climb.Models.League", "League")
                        .WithMany("Seasons")
                        .HasForeignKey("LeagueID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Climb.Models.SeasonLeagueUser", b =>
                {
                    b.HasOne("Climb.Models.LeagueUser", "LeagueUser")
                        .WithMany("Seasons")
                        .HasForeignKey("LeagueUserID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Climb.Models.Season", "Season")
                        .WithMany("Participants")
                        .HasForeignKey("SeasonID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Climb.Models.Set", b =>
                {
                    b.HasOne("Climb.Models.League", "League")
                        .WithMany()
                        .HasForeignKey("LeagueID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Climb.Models.LeagueUser", "Player1")
                        .WithMany("P1Sets")
                        .HasForeignKey("Player1ID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Climb.Models.LeagueUser", "Player2")
                        .WithMany("P2Sets")
                        .HasForeignKey("Player2ID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Climb.Models.Season", "Season")
                        .WithMany("Sets")
                        .HasForeignKey("SeasonID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Climb.Models.Stage", b =>
                {
                    b.HasOne("Climb.Models.Game")
                        .WithMany("Stages")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Climb.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Climb.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Climb.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Climb.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
