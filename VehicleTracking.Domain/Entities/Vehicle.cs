namespace VehicleTracking.Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }
    public DateOnly FirstRegistration { get; set; }
    public required string LicensePlate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<GasBill> GasBills { get; set; } = [];
}