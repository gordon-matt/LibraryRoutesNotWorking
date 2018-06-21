using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Security.Membership
{
    //public enum UserStatus
    //{
    //    NotRegistered = 0,
    //    Unconfirmed = 1,
    //    Locked = 2,
    //    Active = 3
    //}

    public interface IMembershipService
    {
        bool SupportsRolePermissions { get; }

        Task<string> GenerateEmailConfirmationToken(object userId);

        Task ConfirmEmail(object userId, string token);

        #region Users

        //IQueryable<FrameworkUser> GetAllUsersAsQueryable(DbContext context, int? tenantId);

        Task<IEnumerable<FrameworkUser>> GetAllUsers(int? tenantId);

        Task<IEnumerable<FrameworkUser>> GetUsers(int? tenantId, Expression<Func<FrameworkUser, bool>> predicate);

        Task<FrameworkUser> GetUserById(object userId);

        Task<FrameworkUser> GetUserByEmail(int? tenantId, string email);

        Task<FrameworkUser> GetUserByName(int? tenantId, string userName);

        Task<IEnumerable<FrameworkRole>> GetRolesForUser(object userId);

        Task<bool> DeleteUser(object userId);

        Task InsertUser(FrameworkUser user, string password);

        Task UpdateUser(FrameworkUser user);

        Task AssignUserToRoles(int? tenantId, object userId, IEnumerable<object> roleIds);

        Task ChangePassword(object userId, string newPassword);

        Task<string> GetUserDisplayName(FrameworkUser user);

        #endregion Users

        #region Roles

        Task<IEnumerable<FrameworkRole>> GetAllRoles(int? tenantId);

        Task<FrameworkRole> GetRoleById(object roleId);

        Task<IEnumerable<FrameworkRole>> GetRolesByIds(IEnumerable<object> roleIds);

        Task<FrameworkRole> GetRoleByName(int? tenantId, string roleName);

        Task<bool> DeleteRole(object roleId);

        Task InsertRole(FrameworkRole role);

        Task UpdateRole(FrameworkRole role);

        Task<IEnumerable<FrameworkUser>> GetUsersByRoleId(object roleId);

        Task<IEnumerable<FrameworkUser>> GetUsersByRoleName(int? tenantId, string roleName);

        #endregion Roles

        #region Permissions

        Task<IEnumerable<FrameworkPermission>> GetAllPermissions(int? tenantId);

        Task<FrameworkPermission> GetPermissionById(object permissionId);

        Task<FrameworkPermission> GetPermissionByName(int? tenantId, string permissionName);

        Task<IEnumerable<FrameworkPermission>> GetPermissionsForRole(int? tenantId, string roleName);

        Task AssignPermissionsToRole(object roleId, IEnumerable<object> permissionIds);

        Task<bool> DeletePermission(object permissionId);

        Task<bool> DeletePermissions(IEnumerable<object> permissionIds);

        Task InsertPermission(FrameworkPermission permission);

        Task InsertPermissions(IEnumerable<FrameworkPermission> permissions);

        Task UpdatePermission(FrameworkPermission permission);

        #endregion Permissions

        #region Profile

        Task<IDictionary<string, string>> GetProfile(string userId);

        Task<IEnumerable<UserProfile>> GetProfiles(IEnumerable<string> userIds);

        Task UpdateProfile(string userId, IDictionary<string, string> profile, bool deleteExisting = false);

        Task<string> GetProfileEntry(string userId, string key);

        Task SaveProfileEntry(string userId, string key, string value);

        Task DeleteProfileEntry(string userId, string key);

        Task<IEnumerable<FrameworkUserProfileEntry>> GetProfileEntriesByKey(int? tenantId, string key);

        Task<IEnumerable<FrameworkUserProfileEntry>> GetProfileEntriesByKeyAndValue(int? tenantId, string key, string value);

        Task<bool> ProfileEntryExists(int? tenantId, string key, string value, string userId = null);

        #endregion Profile

        Task EnsureAdminRoleForTenant(int? tenantId);
    }

    public class UserProfile
    {
        public string UserId { get; set; }

        public IDictionary<string, string> Profile { get; set; }
    }
}