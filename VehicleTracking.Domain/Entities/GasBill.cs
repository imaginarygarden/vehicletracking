using VehicleTracking.Application.Interfaces;

namespace VehicleTracking.Domain.Entities;

public class GasBill : IDbSetEntity
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int Liters { get; set; }
    public double Price { get; set; }
    public double Mileage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Vehicle Vehicle { get; set; } = null!;
}