using System.Collections.Generic;
using System.Linq;
using Climb.Data;
using Climb.Models;

namespace Climb.Test.Utilities
{
    public static class SeasonUtility
    {
        public static (Season season, List<LeagueUser> members) CreateSeason(ApplicationDbContext dbContext, int participants)
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var members = LeagueUtility.AddUsersToLeague(league, participants, dbContext);

            var season = DbContextUtility.AddNew<Season>(dbContext, s => s.LeagueID = league.ID);
            DbContextUtility.AddNewRange<SeasonLeagueUser>(dbContext, participants, (slu, i) =>
            {
                slu.LeagueUserID = members[i].ID;
                slu.SeasonID = season.ID;
            });

            return (season, members);
        }

        public static List<Set> CreateSets(ApplicationDbContext dbContext, Season season)
        {
            var sets = new List<Set>();
            var participants = season.Participants.ToArray();
            for(var i = 1; i < participants.Length; i++)
            {
                var set = SetUtility.Create(dbContext, participants[0], participants[i], season.LeagueID);
                sets.Add(set);
            }

            return sets;
        }
    }
}