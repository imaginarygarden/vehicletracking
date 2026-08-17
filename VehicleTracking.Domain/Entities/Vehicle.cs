using VehicleTracking.Domain.Interfaces;

namespace VehicleTracking.Domain.Entities;

public class Vehicle : IEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateOnly FirstRegistration { get; set; }
    public required string LicensePlate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
    public ICollection<FuelEntry> FuelEntries { get; set; } = [];
}