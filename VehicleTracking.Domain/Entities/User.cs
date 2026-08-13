using VehicleTracking.Application.Interfaces;
using VehicleTracking.Domain.Enums;

namespace VehicleTracking.Domain.Entities;

public class User : IDbSetEntityActivity
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSeenAt { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = [];
    public ICollection<Session> Sessions { get; set; } = [];
}