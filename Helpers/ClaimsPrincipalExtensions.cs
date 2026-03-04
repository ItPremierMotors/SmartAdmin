using System.Security.Claims;

namespace SmartAdmin.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool TienePermiso(this ClaimsPrincipal user, string permiso)
        {
            return user.HasClaim("Permission", permiso);
        }
    }
}
