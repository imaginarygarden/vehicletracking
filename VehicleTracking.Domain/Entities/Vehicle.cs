using VehicleTracking.Application.Interfaces;

namespace VehicleTracking.Domain.Entities;

public class Vehicle : IDbSetEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateOnly FirstRegistration { get; set; }
    public required string LicensePlate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
    public ICollection<GasBill> GasBills { get; set; } = [];
}