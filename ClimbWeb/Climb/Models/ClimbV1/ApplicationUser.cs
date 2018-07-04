using Microsoft.AspNetCore.Identity;

namespace ClimbV1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int UserID { get; set; }

        public User User { get; set; }
    }
}
