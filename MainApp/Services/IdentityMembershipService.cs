using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Extenso.Collections;
using Extenso.Data.Entity;
using Framework.Security.Membership;
using MainApp.Data.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MainApp.Services
{
    public abstract class IdentityMembershipService : IMembershipService
    {
        private readonly IDbContextFactory contextFactory;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        private static Dictionary<string, List<FrameworkRole>> cachedUserRoles;

        static IdentityMembershipService()
        {
            cachedUserRoles = new Dictionary<string, List<FrameworkRole>>();
        }

        public IdentityMembershipService(
            IDbContextFactory contextFactory,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.contextFactory = contextFactory;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        #region IMembershipService Members

        public bool SupportsRolePermissions
        {
            get { return true; }
        }

        public async Task<string> GenerateEmailConfirmationToken(object userId)
        {
            string id = userId.ToString();
            var user = await userManager.FindByIdAsync(id);
            return await userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task ConfirmEmail(object userId, string token)
        {
            string id = userId.ToString();
            var user = await userManager.FindByIdAsync(id);
            await userManager.ConfirmEmailAsync(user, token);
        }

        #region Users

        // Ignore this.. it was used for OData in MVC5 and will be again in future...
        private IQueryable<FrameworkUser> GetAllUsersAsQueryable(DbContext context)
        {
            IQueryable<ApplicationUser> query = context.Set<ApplicationUser>();

            return query
                .Select(x => new FrameworkUser
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    IsLockedOut = x.LockoutEnabled
                });
        }

        public async Task<IEnumerable<FrameworkUser>> GetAllUsers()
        {
            using (var context = contextFactory.GetContext())
            {
                return await GetAllUsersAsQueryable(context).ToHashSetAsync();
            }
        }

        public async Task<IEnumerable<FrameworkUser>> GetUsers(Expression<Func<FrameworkUser, bool>> predicate)
        {
            using (var context = contextFactory.GetContext())
            {
                return await GetAllUsersAsQueryable(context)
                    .Where(predicate)
                    .ToHashSetAsync();
            }
        }

        public async Task<FrameworkUser> GetUserById(object userId)
        {
            string id = userId.ToString();
            //var user = userManager.FindById(id);

            using (var context = contextFactory.GetContext())
            {
                var user = context.Set<ApplicationUser>().Find(userId);

                if (user == null)
                {
                    return null;
                }

                return await Task.FromResult(new FrameworkUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsLockedOut = user.LockoutEnabled
                });
            }
        }

        public async Task<FrameworkUser> GetUserByEmail(string email)
        {
            ApplicationUser user;

            using (var context = contextFactory.GetContext())
            {
                user = await context.Set<ApplicationUser>().FirstOrDefaultAsync(x => x.Email == email);

                if (user == null)
                {
                    return null;
                }

                return new FrameworkUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsLockedOut = user.LockoutEnabled
                };
            }
        }

        public async Task<FrameworkUser> GetUserByName(string userName)
        {
            ApplicationUser user;

            using (var context = contextFactory.GetContext())
            {
                user = await context.Set<ApplicationUser>().FirstOrDefaultAsync(x => x.UserName == userName);

                if (user == null)
                {
                    return null;
                }

                return new FrameworkUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsLockedOut = user.LockoutEnabled
                };
            }
        }

        public async Task<IEnumerable<FrameworkRole>> GetRolesForUser(object userId)
        {
            string id = userId.ToString();
            if (cachedUserRoles.ContainsKey(id))
            {
                return cachedUserRoles[id];
            }

            var user = await userManager.FindByIdAsync(id);
            var roleNames = await userManager.GetRolesAsync(user);

            var roles = roleManager.Roles
                .Where(x => roleNames.Contains(x.Name))
                .Select(x => new FrameworkRole
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            cachedUserRoles.Add(id, roles);
            return roles;
        }

        public async Task<bool> DeleteUser(object userId)
        {
            string id = userId.ToString();
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                return result.Succeeded;
            }

            return false;
        }

        public async Task InsertUser(FrameworkUser user, string password)
        {
            // Check for spaces in UserName above, because of this:
            // http://stackoverflow.com/questions/30078332/bug-in-asp-net-identitys-usermanager
            string userName = (user.UserName.Contains(" ") ? user.UserName.Replace(" ", "_") : user.UserName);

            var appUser = new ApplicationUser
            {
                UserName = userName,
                Email = user.Email,
                LockoutEnabled = user.IsLockedOut
            };

            var result = await userManager.CreateAsync(appUser, password);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                throw new ApplicationException(errorMessage);
            }
        }

        public async Task UpdateUser(FrameworkUser user)
        {
            string userId = user.Id.ToString();
            var existingUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.LockoutEnabled = user.IsLockedOut;
                var result = await userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                {
                    string errorMessage = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                    throw new ApplicationException(errorMessage);
                }
            }
        }

        public async Task AssignUserToRoles(object userId, IEnumerable<object> roleIds)
        {
            string uId = userId.ToString();

            IQueryable<string> currentRoleIds;

            using (var context = contextFactory.GetContext())
            {
                currentRoleIds = from ur in context.Set<IdentityUserRole<string>>()
                                 join r in context.Set<ApplicationRole>() on ur.RoleId equals r.Id
                                 where ur.UserId == uId
                                 select ur.RoleId;

                var rIds = roleIds.ToListOf<string>();

                var toDelete = from ur in context.Set<IdentityUserRole<string>>()
                               join r in context.Set<ApplicationRole>() on ur.RoleId equals r.Id
                               where ur.UserId == uId
                               && !rIds.Contains(ur.RoleId)
                               select ur;

                var toAdd = rIds.Where(x => !currentRoleIds.Contains(x)).Select(x => new IdentityUserRole<string>
                {
                    UserId = uId,
                    RoleId = x
                });

                if (toDelete.Any())
                {
                    context.Set<IdentityUserRole<string>>().RemoveRange(toDelete);
                }

                if (toAdd.Any())
                {
                    context.Set<IdentityUserRole<string>>().AddRange(toAdd);
                }

                await context.SaveChangesAsync();
            }

            cachedUserRoles.Remove(uId);
        }

        public async Task ChangePassword(object userId, string newPassword)
        {
            //TODO: This doesn't seem to be working very well; no errors, but can't login with the given password
            string id = userId.ToString();
            var user = await userManager.FindByIdAsync(id);
            var result = await userManager.RemovePasswordAsync(user);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                throw new ApplicationException(errorMessage);
            }

            result = await userManager.AddPasswordAsync(user, newPassword);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                throw new ApplicationException(errorMessage);
            }
            //var user = userManager.FindById(id);
            //string passwordHash = userManager.PasswordHasher.HashPassword(newPassword);
            //userStore.SetPasswordHashAsync(user, passwordHash);
            //userManager.UpdateSecurityStamp(id);
        }

        public async Task<string> GetUserDisplayName(FrameworkUser user)
        {
            return user.UserName;
        }

        #endregion Users

        #region Set<ApplicationRole>()

        public async Task<IEnumerable<FrameworkRole>> GetAllRoles()
        {
            IQueryable<ApplicationRole> query = roleManager.Roles;

            return await query
                .Select(x => new FrameworkRole
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<FrameworkRole> GetRoleById(object roleId)
        {
            string id = roleId.ToString();
            var role = await roleManager.FindByIdAsync(id);

            return new FrameworkRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<IEnumerable<FrameworkRole>> GetRolesByIds(IEnumerable<object> roleIds)
        {
            var ids = roleIds.ToListOf<string>();
            var roles = new List<ApplicationRole>();

            foreach (string id in ids)
            {
                var role = await roleManager.FindByIdAsync(id);
                roles.Add(role);
            }

            return roles.Select(x => new FrameworkRole
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public async Task<FrameworkRole> GetRoleByName(string roleName)
        {
            var role = await roleManager.Roles.FirstOrDefaultAsync(x => x.Name == roleName);

            if (role == null)
            {
                return null;
            }

            return new FrameworkRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<bool> DeleteRole(object roleId)
        {
            string id = roleId.ToString();
            var role = await roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                return result.Succeeded;
            }

            return false;
        }

        public async Task InsertRole(FrameworkRole role)
        {
            var result = await roleManager.CreateAsync(new ApplicationRole
            {
                Name = role.Name
            });

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                throw new ApplicationException(errorMessage);
            }
        }

        public async Task UpdateRole(FrameworkRole role)
        {
            string id = role.Id.ToString();
            var existingRole = await roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRole != null)
            {
                existingRole.Name = role.Name;
                var result = await roleManager.UpdateAsync(existingRole);

                if (!result.Succeeded)
                {
                    string errorMessage = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                    throw new ApplicationException(errorMessage);
                }
            }
        }

        public async Task<IEnumerable<FrameworkUser>> GetUsersByRoleId(object roleId)
        {
            string rId = roleId.ToString();

            // TODO: Include(x => x.Users) does not work. Probably because it should map to a junction table first (AspNetUserRoles) and then get the Users from that.
            //      userManager.GetUsersInRoleAsync(role.Name) // <-- probably need a custom UserManager (to take TenantId into account) and use this to get the users
            var role = await roleManager.Roles.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == rId);
            var userIds = role.Users.Select(x => x.Id).ToList();
            var users = await userManager.Users.Where(x => userIds.Contains(x.Id)).ToHashSetAsync();

            return users.Select(x => new FrameworkUser
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                IsLockedOut = x.LockoutEnabled
            });
        }

        public async Task<IEnumerable<FrameworkUser>> GetUsersByRoleName(string roleName)
        {
            ApplicationRole role;

            role = await roleManager.Roles
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Name == roleName);

            var userIds = role.Users.Select(x => x.Id).ToList();
            var users = await userManager.Users.Where(x => userIds.Contains(x.Id)).ToHashSetAsync();

            return users.Select(x => new FrameworkUser
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                IsLockedOut = x.LockoutEnabled
            });
        }

        #endregion Set<ApplicationRole>()

        #endregion IMembershipService Members
    }
}