using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClimbV1.Models
{
    public class User : IProfile
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string ProfilePicKey { get; set; }
        public string BannerPicKey { get; set; }

        public HashSet<LeagueUser> LeagueUsers { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}