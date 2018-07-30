using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Security.Membership
{
    public interface IMembershipService
    {
        bool SupportsRolePermissions { get; }

        Task<string> GenerateEmailConfirmationToken(object userId);

        Task ConfirmEmail(object userId, string token);

        #region Users
        
        Task<IEnumerable<FrameworkUser>> GetAllUsers();

        Task<IEnumerable<FrameworkUser>> GetUsers(Expression<Func<FrameworkUser, bool>> predicate);

        Task<FrameworkUser> GetUserById(object userId);

        Task<FrameworkUser> GetUserByEmail(string email);

        Task<FrameworkUser> GetUserByName(string userName);

        Task<IEnumerable<FrameworkRole>> GetRolesForUser(object userId);

        Task<bool> DeleteUser(object userId);

        Task InsertUser(FrameworkUser user, string password);

        Task UpdateUser(FrameworkUser user);

        Task AssignUserToRoles(object userId, IEnumerable<object> roleIds);

        Task ChangePassword(object userId, string newPassword);

        Task<string> GetUserDisplayName(FrameworkUser user);

        #endregion Users

        #region Roles

        Task<IEnumerable<FrameworkRole>> GetAllRoles();

        Task<FrameworkRole> GetRoleById(object roleId);

        Task<IEnumerable<FrameworkRole>> GetRolesByIds(IEnumerable<object> roleIds);

        Task<FrameworkRole> GetRoleByName(string roleName);

        Task<bool> DeleteRole(object roleId);

        Task InsertRole(FrameworkRole role);

        Task UpdateRole(FrameworkRole role);

        Task<IEnumerable<FrameworkUser>> GetUsersByRoleId(object roleId);

        Task<IEnumerable<FrameworkUser>> GetUsersByRoleName(string roleName);

        #endregion Roles
    }

    public class UserProfile
    {
        public string UserId { get; set; }

        public IDictionary<string, string> Profile { get; set; }
    }
}