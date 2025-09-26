using System.Security.Claims;

namespace Mehrzad.SmartCampus.Backend.Security.API.Handlers
{
    public static class ValidateTokenHandler
    {
        public static IResult Handle(HttpContext context)
        {
            // Middleware has already validated the token if present
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                return Results.Ok(new { isOK = false, role = (string?)null });
            }

            var roleClaim = context.User.FindFirst(ClaimTypes.Role) ?? context.User.FindFirst("role");
            var role = roleClaim?.Value;

            return Results.Ok(new { isOK = true, role });
        }
    }
}
