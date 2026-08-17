using System.Security.Claims;
using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Domain.Enums;

namespace VehicleTracking.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static SessionDto? GetSession(this ClaimsPrincipal claimsPrincipal)
    {
        var claims = claimsPrincipal.Claims;
        var enumerable = claims as Claim[] ?? claims.ToArray();
        var isAnyNull = enumerable.Any(c => string.IsNullOrWhiteSpace(c.Value));
        
        var dummyClaims = SessionDto.Dummy.Claims;
        var isMatchedOrDefault = enumerable?.All(i => dummyClaims.Any(j => j.Type == i.Type)) ?? false;

        var isVerified = !isAnyNull && isMatchedOrDefault;

        if (isVerified && enumerable != null)
        {
            var userId = enumerable.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var username = enumerable.First(c => c.Type == ClaimTypes.Name).Value;
            var role = enumerable.First(c => c.Type == ClaimTypes.Role).Value;
            var sessionId = enumerable.First(c => c.Type == ClaimTypes.SerialNumber).Value;
            var issuedAt = enumerable.First(c => c.Type == ClaimTypes.DateOfBirth).Value;
        
            return new SessionDto(userId, username, role, sessionId, issuedAt);
        }

        return null;
    }
    
    public static UserRole? GetUserRole(this ClaimsPrincipal claimsPrincipal)
    {
        var roleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role);
        
        if (roleClaim == null)
            return null;

        if (!Enum.TryParse<UserRole>(roleClaim.Value, out var role))
            return null;

        return role;
    }
}