﻿using System.Collections.Generic;
using Climb.Data;
using Climb.Models;

namespace Climb.Test.Utilities
{
    public static class LeagueUtility
    {
        public static League CreateLeague(ApplicationDbContext dbContext)
        {
            var game = DbContextUtility.AddNew<Game>(dbContext);
            var league = DbContextUtility.AddNew<League>(dbContext, l => l.GameID = game.ID);
            return league;
        }

        public static List<LeagueUser> AddUsersToLeague(League league, int count, ApplicationDbContext dbContext)
        {
            return DbContextUtility.AddNewRange<LeagueUser>(dbContext, count, (lu, i) => lu.LeagueID = league.ID);
        }
    }
}