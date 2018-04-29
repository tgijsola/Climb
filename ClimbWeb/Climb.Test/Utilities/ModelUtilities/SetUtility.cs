using Climb.Data;
using Climb.Models;

namespace Climb.Test.Utilities
{
    public static class SetUtility
    {
        public static Set Create(ApplicationDbContext dbContext, int player1ID, int player2ID, Season season)
        {
            var set = DbContextUtility.AddNew<Set>(dbContext, s =>
            {
                s.Player1ID = player1ID;
                s.Player2ID = player2ID;
                s.LeagueID = season.LeagueID;
                s.SeasonID = season.ID;
            });

            return set;
        }
    }
}