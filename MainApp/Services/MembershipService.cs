using Extenso.Data.Entity;
using FrameworkDemo.Data.Domain;
using Microsoft.AspNetCore.Identity;

namespace FrameworkDemo.Services
{
    public class MembershipService : IdentityMembershipService
    {
        public MembershipService(
            IDbContextFactory contextFactory,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IRepository<UserProfileEntry> userProfileRepository)
            : base(contextFactory, userManager, roleManager, userProfileRepository)
        {
        }
    }
}