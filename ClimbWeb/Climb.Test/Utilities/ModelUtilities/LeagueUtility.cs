﻿using System.Collections.Generic;
using Climb.Data;
using Climb.Models;

namespace Climb.Test.Utilities
{
    public static class LeagueUtility
    {
        public static League CreateLeague(ApplicationDbContext dbContext, string adminID = null)
        {
            var game = DbContextUtility.AddNew<Game>(dbContext);
            var league = DbContextUtility.AddNew<League>(dbContext, l =>
            {
                l.GameID = game.ID;
                if(!string.IsNullOrWhiteSpace(adminID))
                {
                    l.AdminID = adminID;
                }
            });
            return league;
        }

        public static List<LeagueUser> AddUsersToLeague(League league, int count, ApplicationDbContext dbContext)
        {
            var users = DbContextUtility.AddNewRange<ApplicationUser>(dbContext, count);
            return DbContextUtility.AddNewRange<LeagueUser>(dbContext, count, (lu, i) =>
            {
                lu.UserID = users[i].Id;
                lu.LeagueID = league.ID;
            });
        }
    }
}