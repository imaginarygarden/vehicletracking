using System.Security.Claims;
using VehicleTracking.Domain.Enums;

namespace VehicleTracking.Application.Models.Authentication;

public record SessionDto(string UserId, string Username, string Role, string SessionId, string IssuedAt)
{
    public Claim[] Claims => 
        new[]
        {
            new Claim(ClaimTypes.NameIdentifier, UserId),
            new Claim(ClaimTypes.Name, Username),
            new Claim(ClaimTypes.Role, Role),
            new Claim(ClaimTypes.SerialNumber, SessionId),
            new Claim(ClaimTypes.DateOfBirth, IssuedAt),
        };

    public static SessionDto Dummy => new SessionDto("dummy", "dummy", "User", "dummy", "1786630651");

    public static SessionDto FromClaims(IEnumerable<Claim> claims)
    {
        var enumerable = claims as Claim[] ?? claims.ToArray();
        
        var userId = enumerable.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var username = enumerable.First(c => c.Type == ClaimTypes.Name).Value;
        var role = enumerable.First(c => c.Type == ClaimTypes.Role).Value;
        var sessionId = enumerable.First(c => c.Type == ClaimTypes.SerialNumber).Value;
        var issuedAt = enumerable.First(c => c.Type == ClaimTypes.DateOfBirth).Value;
        
        return new SessionDto(userId, username, role, sessionId, issuedAt);
    }
}