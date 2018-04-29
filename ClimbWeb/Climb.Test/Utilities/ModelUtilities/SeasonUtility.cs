using Climb.Data;
using Climb.Models;

namespace Climb.Test.Utilities
{
    public static class SeasonUtility
    {
        public static Season CreateSeason(ApplicationDbContext dbContext, int participants)
        {
            var league = LeagueUtility.CreateLeague(dbContext);
            var members = LeagueUtility.AddUsersToLeague(league, participants, dbContext);

            var season = DbContextUtility.AddNew<Season>(dbContext, s => s.LeagueID = league.ID);
            DbContextUtility.AddNewRange<SeasonLeagueUser>(dbContext, participants, (slu, i) =>
            {
                slu.LeagueUserID = members[i].ID;
                slu.SeasonID = season.ID;
            });

            return season;
        }
    }
}