﻿using System.Collections.Generic;
using Climb.Models;
using Microsoft.AspNetCore.Identity;

namespace Climb.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicKey { get; set; }

        public List<LeagueUser> LeagueUsers { get; set; }
    }
}