using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;

namespace Authen.Users.Extensions
{
    public static class UserExtensions
    {
        public static string GetSubjectById(this IPrincipal principal)
        {
            return principal.Identity.GetSubjectById();
        }

        public static string GetSubjectById(this IIdentity identity)
        {
            var id = identity as ClaimsIdentity;
            var claim = id.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null) throw new InvalidOperationException("sub claim is missing");
            return claim.Value;
        }

        public static string GetDisplayByName(this ClaimsPrincipal principal)
        {
            var name = principal.Identity.Name;
            if (!string.IsNullOrWhiteSpace(name)) return name;

            var sub = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (sub != null) return sub.Value;

            return string.Empty;
        }

        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
                return new Guid(user.GetSubjectById());
            else
                return new Guid();
        }

        public static string GetErrors(this IdentityResult result)
        {
            return string.Join("\n", result.Errors.Select(i => i.Description));
        }
    }


}