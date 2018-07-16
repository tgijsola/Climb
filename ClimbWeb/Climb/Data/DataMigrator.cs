using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ClimbV1.Models;
using Microsoft.EntityFrameworkCore;

namespace Climb.Data
{
    public static class DataMigrator
    {
        private static readonly Dictionary<string, string> applicationUserIDs = new Dictionary<string, string>();
        private static readonly Dictionary<int, int> gameIDs = new Dictionary<int, int>();
        private static readonly Dictionary<int, int> leagueIDs = new Dictionary<int, int>();
        private static readonly Dictionary<int, int> leagueUserIDs = new Dictionary<int, int>();
        private static readonly Dictionary<int, int> seasonIDs = new Dictionary<int, int>();
        private static readonly Dictionary<int, int> setIDs = new Dictionary<int, int>();

        public static async Task MigrateV1(ApplicationDbContext context)
        {
            await ResetDatabase(context);

            ClimbV1Context v1Context = CreateDB();

            await MigrateUsers(v1Context, context);
            await MigrateGames(v1Context, context);
            await MigrateCharacters(v1Context, context);
            await MigrateStages(v1Context, context);
            await MigrateLeagues(v1Context, context);
            await MigrateSeasons(v1Context, context);
            await MigrateLeagueUsers(v1Context, context);
            await MigrateSets(v1Context, context);
            await MigrateMatches(v1Context, context);
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

            for(var i = 0; i < users.Length; i++)
            {
                applicationUserIDs[v1Users[i].ApplicationUser.Id] = users[i].Id;
            }
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

        private static async Task MigrateCharacters(ClimbV1Context v1Context, ApplicationDbContext context)
        {
            var oldCharacters = await v1Context.Character.ToArrayAsync();
            var characters = new Models.Character[oldCharacters.Length];

            for(int i = 0; i < oldCharacters.Length; i++)
            {
                var v1Character = oldCharacters[i];
                characters[i] = new Models.Character
                {
                    Name = v1Character.Name,
                    GameID = gameIDs[v1Character.GameID],
                };
            }

            context.Characters.AddRange(characters);
            await context.SaveChangesAsync();
        }
        
        private static async Task MigrateStages(ClimbV1Context v1Context, ApplicationDbContext context)
        {
            var oldStages = await v1Context.Stage.ToArrayAsync();
            var stages = new Models.Stage[oldStages.Length];

            for(int i = 0; i < oldStages.Length; i++)
            {
                var v1Stage = oldStages[i];
                stages[i] = new Models.Stage
                {
                    Name = v1Stage.Name,
                    GameID = gameIDs[v1Stage.GameID],
                };
            }

            context.Stages.AddRange(stages);
            await context.SaveChangesAsync();
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

            for(int i = 0; i < oldLeagues.Length; i++)
            {
                leagueIDs[oldLeagues[i].ID] = leagues[i].ID;
            }
        }

        private static async Task MigrateSeasons(ClimbV1Context v1Context, ApplicationDbContext context)
        {
            var oldSeasons = await v1Context.Season.ToArrayAsync();
            var seasons = new Models.Season[oldSeasons.Length];

            for(int i = 0; i < oldSeasons.Length; i++)
            {
                var oldSeason = oldSeasons[i];
                seasons[i] = new Models.Season
                {
                    LeagueID = leagueIDs[oldSeason.LeagueID],
                    StartDate = oldSeason.StartDate,
                    EndDate = oldSeason.StartDate + TimeSpan.FromDays(30),
                    Index = oldSeason.Index,
                    IsActive = !oldSeason.IsComplete,
                };
            }

            context.Seasons.AddRange(seasons);
            await context.SaveChangesAsync();
            
            for(int i = 0; i < oldSeasons.Length; i++)
            {
                seasonIDs[oldSeasons[i].ID] = seasons[i].ID;
            }
        }

        private static async Task MigrateLeagueUsers(ClimbV1Context v1Context, ApplicationDbContext context)
        {
            var oldLeagueUsers = await v1Context.LeagueUser
                .Include(lu => lu.User).ThenInclude(u => u.ApplicationUser).AsNoTracking()
                .ToArrayAsync();
            var leagueUsers = new Models.LeagueUser[oldLeagueUsers.Length];

            for(int i = 0; i < oldLeagueUsers.Length; i++)
            {
                var oldLeagueUser = oldLeagueUsers[i];
                leagueUsers[i] = new Models.LeagueUser
                {
                    LeagueID = leagueIDs[oldLeagueUser.LeagueID],
                    UserID = applicationUserIDs[oldLeagueUser.User.ApplicationUser.Id],
                    HasLeft = oldLeagueUser.HasLeft,
                    Rank = oldLeagueUser.Rank,
                    Points = oldLeagueUser.Points,
                    SetCount = oldLeagueUser.SetsPlayed,
                };
            }

            context.LeagueUsers.AddRange(leagueUsers);
            await context.SaveChangesAsync();

            for(int i = 0; i < oldLeagueUsers.Length; i++)
            {
                leagueUserIDs[oldLeagueUsers[i].ID] = leagueUsers[i].ID;
            }
        }

        private static async Task MigrateSets(ClimbV1Context v1Context, ApplicationDbContext context)
        {
            var oldSets = await v1Context.Set.ToArrayAsync();
            var sets = new Models.Set[oldSets.Length];

            for(int i = 0; i < oldSets.Length; i++)
            {
                var oldSet = oldSets[i];
                Debug.Assert(oldSet.Player1ID != null, "oldSet.Player1ID != null");
                Debug.Assert(oldSet.Player2ID != null, "oldSet.Player2ID != null");
                var isComplete = oldSet.Player1Score != null;
                var player1Won = oldSet.Player1Score > oldSet.Player2Score;
                var player2Won = oldSet.Player2Score > oldSet.Player1Score;
                sets[i] = new Models.Set
                {
                    LeagueID = leagueIDs[oldSet.LeagueID],
                    DueDate = oldSet.DueDate,
                    UpdatedDate = oldSet.UpdatedDate,
                    IsLocked = oldSet.IsLocked,
                    Player1ID = leagueUserIDs[oldSet.Player1ID.Value],
                    Player2ID = leagueUserIDs[oldSet.Player2ID.Value],
                    SeasonID = oldSet.SeasonID != null ? seasonIDs[oldSet.SeasonID.Value] : (int?)null,
                    Player1Score = oldSet.Player1Score,
                    Player2Score = oldSet.Player2Score,
                    IsComplete = isComplete,
                    Player1SeasonPoints = isComplete ? player1Won ? 2 : 1 : 0,
                    Player2SeasonPoints = isComplete ? player2Won ? 2 : 1 : 0,
                    // TODO: SeasonPlayers
                };
            }

            context.Sets.AddRange(sets);
            await context.SaveChangesAsync();
            
            for(int i = 0; i < oldSets.Length; i++)
            {
                setIDs[oldSets[i].ID] = sets[i].ID;
            }
        }
        
        private static async Task MigrateMatches(ClimbV1Context v1Context, ApplicationDbContext context)
        {
            var oldMatches = await v1Context.Match
                .Include(m => m.Set).AsNoTracking()
                .ToArrayAsync();
            var matches = new Models.Match[oldMatches.Length];

            for(int i = 0; i < oldMatches.Length; i++)
            {
                var oldMatch = oldMatches[i];
                matches[i] = new Models.Match
                {
                    Index = oldMatch.Index,
                    SetID = setIDs[oldMatch.Set.ID],
                    Player1Score = oldMatch.Player1Score,
                    Player2Score = oldMatch.Player2Score,
                    StageID = oldMatch.StageID,
                };
            }

            context.Matches.AddRange(matches);
            await context.SaveChangesAsync();
        }
    }
}
