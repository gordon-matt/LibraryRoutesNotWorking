using Framework.Identity;
using Framework.Identity.Domain;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink<TUser>(this IUrlHelper urlHelper, string userId, string code, string scheme)
            where TUser : FrameworkIdentityUser, new()
        {
            return urlHelper.Action(
                action: nameof(FrameworkAccountController<TUser>.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink<TUser>(this IUrlHelper urlHelper, string userId, string code, string scheme)
            where TUser : FrameworkIdentityUser, new()
        {
            return urlHelper.Action(
                action: nameof(FrameworkAccountController<TUser>.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}