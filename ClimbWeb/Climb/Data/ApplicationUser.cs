using System.Collections.Generic;
using System.Linq;
using Climb.Models;
using Climb.Services;
using Microsoft.AspNetCore.Identity;

namespace Climb.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicKey { get; set; }
        public string Name { get; set; }

        public List<OrganizationUser> Organizations { get; set; }
        public List<LeagueUser> LeagueUsers { get; set; }

        public string GetProfilePicUrl(ICdnService cdnService)
        {
            const int defaultPicturesCount = 24;

            if(string.IsNullOrWhiteSpace(ProfilePicKey))
            {
                var idHash = Id.Select(c => (int)c).Sum();
                var defaultID = idHash % defaultPicturesCount;
                return $"/images/profile-default/profile-default-{defaultID}.jpg";
            }

            return cdnService.GetImageUrl(ProfilePicKey, ClimbImageRules.ProfilePic);
        }
    }
}