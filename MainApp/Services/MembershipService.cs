using Extenso.Data.Entity;
using MainApp.Data.Domain;
using Microsoft.AspNetCore.Identity;

namespace MainApp.Services
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