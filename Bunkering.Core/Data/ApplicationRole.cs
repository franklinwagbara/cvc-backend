using Microsoft.AspNetCore.Identity;

namespace Bunkering.Core.Data
{
    public class ApplicationRole : IdentityRole
    {
        public string?  Description { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}