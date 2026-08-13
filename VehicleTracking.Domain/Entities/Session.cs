using VehicleTracking.Application.Interfaces;

namespace VehicleTracking.Domain.Entities;

public class Session : IDbSetEntityActivity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string UserAgent { get; set; }
    public required string IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSeenAt { get; set; }
    
    public User User { get; set; } = null!;
}