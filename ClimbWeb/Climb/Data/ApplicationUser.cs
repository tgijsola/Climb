using Microsoft.AspNetCore.Identity;

namespace Climb.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicKey { get; set; }
    }
}