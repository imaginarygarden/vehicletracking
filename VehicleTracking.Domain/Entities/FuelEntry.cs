using VehicleTracking.Domain.Interfaces;

namespace VehicleTracking.Domain.Entities;

public class FuelEntry : IEntity
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime RefueledAt { get; set; }
    public int Liters { get; set; }
    public double Price { get; set; }
    public int Odometer { get; set; }
    public bool FullTank { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Vehicle Vehicle { get; set; } = null!;
}