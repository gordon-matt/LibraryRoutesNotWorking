//using Framework.DI;
using Framework.Security.Membership;

namespace Framework.Web.Security.Membership.Permissions
{
    /// <summary>
    /// Entry-point for configured authorization scheme. Role-based system provided by default.
    /// </summary>
    public interface IAuthorizationService //: IDependency
    {
        void CheckAccess(Permission permission, FrameworkUser user);

        bool TryCheckAccess(Permission permission, FrameworkUser user);
    }
}