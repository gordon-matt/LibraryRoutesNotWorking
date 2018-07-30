using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Extenso.Collections;
using Extenso.Data.Entity;
using Framework;
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
        private readonly IRepository<UserProfileEntry> userProfileRepository;

        private static Dictionary<string, List<FrameworkRole>> cachedUserRoles;
        private static Dictionary<string, List<FrameworkPermission>> cachedRolePermissions;

        static IdentityMembershipService()
        {
            cachedUserRoles = new Dictionary<string, List<FrameworkRole>>();
            cachedRolePermissions = new Dictionary<string, List<FrameworkPermission>>();
        }

        public IdentityMembershipService(
            IDbContextFactory contextFactory,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IRepository<UserProfileEntry> userProfileRepository)
        {
            this.contextFactory = contextFactory;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userProfileRepository = userProfileRepository;
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
        private IQueryable<FrameworkUser> GetAllUsersAsQueryable(DbContext context, int? tenantId)
        {
            IQueryable<ApplicationUser> query = context.Set<ApplicationUser>();

            if (tenantId.HasValue)
            {
                query = query.Where(x => x.TenantId == tenantId);
            }
            else
            {
                query = query.Where(x => x.TenantId == null);
            }

            return query
                .Select(x => new FrameworkUser
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    UserName = x.UserName,
                    Email = x.Email,
                    IsLockedOut = x.LockoutEnabled
                });
        }

        public async Task<IEnumerable<FrameworkUser>> GetAllUsers(int? tenantId)
        {
            using (var context = contextFactory.GetContext())
            {
                return await GetAllUsersAsQueryable(context, tenantId).ToHashSetAsync();
            }
        }

        public async Task<IEnumerable<FrameworkUser>> GetUsers(int? tenantId, Expression<Func<FrameworkUser, bool>> predicate)
        {
            using (var context = contextFactory.GetContext())
            {
                return await GetAllUsersAsQueryable(context, tenantId)
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
                    TenantId = user.TenantId,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsLockedOut = user.LockoutEnabled
                });
            }
        }

        public async Task<FrameworkUser> GetUserByEmail(int? tenantId, string email)
        {
            ApplicationUser user;

            using (var context = contextFactory.GetContext())
            {
                if (tenantId.HasValue)
                {
                    user = await context.Set<ApplicationUser>().FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Email == email);
                }
                else
                {
                    user = await context.Set<ApplicationUser>().FirstOrDefaultAsync(x => x.TenantId == null && x.Email == email);
                }

                if (user == null)
                {
                    return null;
                }

                return new FrameworkUser
                {
                    Id = user.Id,
                    TenantId = user.TenantId,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsLockedOut = user.LockoutEnabled
                };
            }
        }

        public async Task<FrameworkUser> GetUserByName(int? tenantId, string userName)
        {
            ApplicationUser user;

            using (var context = contextFactory.GetContext())
            {
                if (tenantId.HasValue)
                {
                    user = await context.Set<ApplicationUser>().FirstOrDefaultAsync(x => x.TenantId == tenantId && x.UserName == userName);
                }
                else
                {
                    user = await context.Set<ApplicationUser>().FirstOrDefaultAsync(x => x.TenantId == null && x.UserName == userName);
                }

                if (user == null)
                {
                    return null;
                }

                return new FrameworkUser
                {
                    Id = user.Id,
                    TenantId = user.TenantId,
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
                TenantId = user.TenantId,
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

        //public async Task AssignUserToRoles(object userId, IEnumerable<object> roleIds)
        //{
        //    string uId = userId.ToString();

        //    var ids = roleIds.Select(x => Convert.ToString(x));
        //    var roleNames = await roleManager.Roles.Where(x => ids.Contains(x.Id)).Select(x => x.Name).ToListAsync();

        //    var currentRoles = await userManager.GetRolesAsync(uId);

        //    var toDelete = currentRoles.Where(x => !roleNames.Contains(x));
        //    var toAdd = roleNames.Where(x => !currentRoles.Contains(x));

        //    if (toDelete.Any())
        //    {
        //        foreach (string roleName in toDelete)
        //        {
        //            var result = await userManager.RemoveFromRoleAsync(uId, roleName);

        //            if (!result.Succeeded)
        //            {
        //                string errorMessage = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
        //                throw new ApplicationException(errorMessage);
        //            }
        //        }
        //        cachedUserRoles.Remove(uId);
        //    }

        //    if (toAdd.Any())
        //    {
        //        foreach (string roleName in toAdd)
        //        {
        //            var result = await userManager.AddToRoleAsync(uId, roleName);

        //            if (!result.Succeeded)
        //            {
        //                string errorMessage = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
        //                throw new ApplicationException(errorMessage);
        //            }
        //        }
        //        cachedUserRoles.Remove(uId);
        //    }
        //}

        public async Task AssignUserToRoles(int? tenantId, object userId, IEnumerable<object> roleIds)
        {
            string uId = userId.ToString();

            IQueryable<string> currentRoleIds;

            using (var context = contextFactory.GetContext())
            {
                if (tenantId.HasValue)
                {
                    currentRoleIds = from ur in context.Set<IdentityUserRole<string>>()
                                     join r in context.Set<ApplicationRole>() on ur.RoleId equals r.Id
                                     where r.TenantId == tenantId && ur.UserId == uId
                                     select ur.RoleId;
                }
                else
                {
                    currentRoleIds = from ur in context.Set<IdentityUserRole<string>>()
                                     join r in context.Set<ApplicationRole>() on ur.RoleId equals r.Id
                                     where r.TenantId == null && ur.UserId == uId
                                     select ur.RoleId;
                }

                var rIds = roleIds.ToListOf<string>();

                var toDelete = from ur in context.Set<IdentityUserRole<string>>()
                               join r in context.Set<ApplicationRole>() on ur.RoleId equals r.Id
                               where r.TenantId == tenantId
                               && ur.UserId == uId
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

        public async Task<IEnumerable<FrameworkRole>> GetAllRoles(int? tenantId)
        {
            IQueryable<ApplicationRole> query = roleManager.Roles;

            if (tenantId.HasValue)
            {
                query = query.Where(x => x.TenantId == tenantId);
            }
            else
            {
                query = query.Where(x => x.TenantId == null);
            }

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

        public async Task<FrameworkRole> GetRoleByName(int? tenantId, string roleName)
        {
            ApplicationRole role;

            if (tenantId.HasValue)
            {
                role = await roleManager.Roles.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Name == roleName);
            }
            else
            {
                role = await roleManager.Roles.FirstOrDefaultAsync(x => x.TenantId == null && x.Name == roleName);
            }

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
                TenantId = role.TenantId,
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
                TenantId = x.TenantId,
                UserName = x.UserName,
                Email = x.Email,
                IsLockedOut = x.LockoutEnabled
            });
        }

        public async Task<IEnumerable<FrameworkUser>> GetUsersByRoleName(int? tenantId, string roleName)
        {
            ApplicationRole role;

            if (tenantId.HasValue)
            {
                role = await roleManager.Roles
                    .Include(x => x.Users)
                    .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Name == roleName);
            }
            else
            {
                role = await roleManager.Roles
                    .Include(x => x.Users)
                    .FirstOrDefaultAsync(x => x.TenantId == null && x.Name == roleName);
            }

            var userIds = role.Users.Select(x => x.Id).ToList();
            var users = await userManager.Users.Where(x => userIds.Contains(x.Id)).ToHashSetAsync();

            return users.Select(x => new FrameworkUser
            {
                Id = x.Id,
                TenantId = x.TenantId,
                UserName = x.UserName,
                Email = x.Email,
                IsLockedOut = x.LockoutEnabled
            });
        }

        #endregion Set<ApplicationRole>()
        
        #region Profile

        public async Task<IDictionary<string, string>> GetProfile(string userId)
        {
            using (var connection = userProfileRepository.OpenConnection())
            {
                return await connection.Query(x => x.UserId == userId).ToDictionaryAsync(k => k.Key, v => v.Value);
            }
        }

        public async Task<IEnumerable<UserProfile>> GetProfiles(IEnumerable<string> userIds)
        {
            using (var connection = userProfileRepository.OpenConnection())
            {
                var entries = await connection.Query(x => userIds.Contains(x.UserId)).ToListAsync();
                return entries.GroupBy(x => x.UserId).Select(x => new UserProfile
                {
                    UserId = x.Key,
                    Profile = x.ToDictionary(k => k.Key, v => v.Value)
                });
            }
        }

        public async Task UpdateProfile(string userId, IDictionary<string, string> profile, bool deleteExisting = false)
        {
            List<UserProfileEntry> entries = null;
            using (var connection = userProfileRepository.OpenConnection())
            {
                entries = await connection.Query(x => x.UserId == userId).ToListAsync();
            }

            if (deleteExisting)
            {
                await userProfileRepository.DeleteAsync(entries);

                var toInsert = profile.Select(x => new UserProfileEntry
                {
                    UserId = userId,
                    Key = x.Key,
                    Value = x.Value
                }).ToList();

                await userProfileRepository.InsertAsync(toInsert);
            }
            else
            {
                var toUpdate = new List<UserProfileEntry>();
                var toInsert = new List<UserProfileEntry>();

                foreach (var keyValue in profile)
                {
                    var existing = entries.FirstOrDefault(x => x.Key == keyValue.Key);

                    if (existing != null)
                    {
                        existing.Value = keyValue.Value;
                        toUpdate.Add(existing);
                    }
                    else
                    {
                        toInsert.Add(new UserProfileEntry
                        {
                            UserId = userId,
                            Key = keyValue.Key,
                            Value = keyValue.Value
                        });
                    }
                }

                if (toUpdate.Any())
                {
                    await userProfileRepository.UpdateAsync(toUpdate);
                }

                if (toInsert.Any())
                {
                    await userProfileRepository.InsertAsync(toInsert);
                }
            }
        }

        public async Task<string> GetProfileEntry(string userId, string key)
        {
            var entry = await userProfileRepository.FindOneAsync(x =>
                x.UserId == userId &&
                x.Key == key);

            if (entry != null)
            {
                return entry.Value;
            }

            return null;
        }

        public async Task SaveProfileEntry(string userId, string key, string value)
        {
            var entry = await userProfileRepository.FindOneAsync(x =>
                x.UserId == userId &&
                x.Key == key);

            if (entry != null)
            {
                entry.Value = value;
                await userProfileRepository.UpdateAsync(entry);
            }
            else
            {
                await userProfileRepository.InsertAsync(new UserProfileEntry
                {
                    UserId = userId,
                    Key = key,
                    Value = value
                });
            }
        }

        public async Task DeleteProfileEntry(string userId, string key)
        {
            var entry = await userProfileRepository.FindOneAsync(x =>
                x.UserId == userId &&
                x.Key == key);

            if (entry != null)
            {
                await userProfileRepository.DeleteAsync(entry);
            }
        }

        public async Task<IEnumerable<FrameworkUserProfileEntry>> GetProfileEntriesByKey(int? tenantId, string key)
        {
            using (var connection = userProfileRepository.OpenConnection())
            {
                var query = connection.Query();

                if (tenantId.HasValue)
                {
                    query = query.Where(x => x.TenantId == tenantId && x.Key == key);
                }
                else
                {
                    query = query.Where(x => x.TenantId == null && x.Key == key);
                }

                return (await query.ToHashSetAsync())
                    .Select(x => new FrameworkUserProfileEntry
                    {
                        Id = x.Id.ToString(),
                        UserId = x.UserId,
                        Key = x.Key,
                        Value = x.Value
                    });
            }
        }

        public async Task<IEnumerable<FrameworkUserProfileEntry>> GetProfileEntriesByKeyAndValue(int? tenantId, string key, string value)
        {
            using (var connection = userProfileRepository.OpenConnection())
            {
                var query = connection.Query();

                if (tenantId.HasValue)
                {
                    query = query.Where(x => x.TenantId == tenantId && x.Key == key && x.Value == value);
                }
                else
                {
                    query = query.Where(x => x.TenantId == null && x.Key == key && x.Value == value);
                }

                return (await query.ToHashSetAsync())
                    .Select(x => new FrameworkUserProfileEntry
                    {
                        Id = x.Id.ToString(),
                        UserId = x.UserId,
                        Key = x.Key,
                        Value = x.Value
                    });
            }
        }

        public async Task<bool> ProfileEntryExists(int? tenantId, string key, string value, string userId = null)
        {
            using (var connection = userProfileRepository.OpenConnection())
            {
                IQueryable<UserProfileEntry> query = null;

                if (tenantId.HasValue)
                {
                    query = connection.Query(x => x.TenantId == tenantId && x.Key == key && x.Value == value);
                }
                else
                {
                    query = connection.Query(x => x.TenantId == null && x.Key == key && x.Value == value);
                }

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.UserId == userId);
                }
                return await query.AnyAsync();
            }
        }

        #endregion Profile

        public async Task EnsureAdminRoleForTenant(int? tenantId)
        {
            if (SupportsRolePermissions)
            {
                var administratorsRole = await GetRoleByName(tenantId, FrameworkConstants.Roles.Administrators);
                if (administratorsRole == null)
                {
                    await InsertRole(new FrameworkRole { TenantId = tenantId, Name = FrameworkConstants.Roles.Administrators });
                    administratorsRole = await GetRoleByName(tenantId, FrameworkConstants.Roles.Administrators);

                    if (administratorsRole != null)
                    {
                        // Assign all super admin users (NULL TenantId) to this new admin role
                        var superAdminUsers = await GetUsersByRoleName(null, FrameworkConstants.Roles.Administrators);
                        foreach (var user in superAdminUsers)
                        {
                            await AssignUserToRoles(tenantId, user.Id, new[] { administratorsRole.Id });
                        }
                    }
                }
            }
        }

        #endregion IMembershipService Members
    }
}