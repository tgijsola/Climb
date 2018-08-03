using System.Collections.Generic;
using System.Linq;
using Climb.Data;
using Climb.Models;

namespace Climb.ViewModels.Organizations
{
    public class HomeViewModel : BaseViewModel
    {
        public Organization Organization { get; }
        public bool IsMember { get; }
        public IReadOnlyList<OrganizationUser> Members { get; }

        public HomeViewModel(ApplicationUser user, Organization organization)
            : base(user)
        {
            Organization = organization;

            organization.Members.Sort();
            Members = organization.Members;
            IsMember = organization.Members.Any(ou => ou.UserID == user?.Id);
        }
    }
}