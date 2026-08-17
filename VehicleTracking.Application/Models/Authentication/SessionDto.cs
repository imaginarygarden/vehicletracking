using System.Security.Claims;
using VehicleTracking.Domain.Entities;
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

    public static SessionDto Dummy => new ("dummy", "dummy", "User", "dummy", "1786630651");

    public static SessionDto FromSession(Session session, TimeProvider timeProvider)
    {
        return new SessionDto(session.UserId.ToString(), session.User.Username, session.User.Role.ToString(), session.Id.ToString(), $"{timeProvider.GetUtcNow().ToUnixTimeSeconds()}");
    }
}