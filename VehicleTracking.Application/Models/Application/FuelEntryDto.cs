using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Models.Application;

public record FuelEntryDto(
    Guid Id, 
    DateTime RefueledAt,
    int Liters, 
    double Price, 
    int Odometer, 
    bool FullTank,
    DateTime CreatedAt, 
    DateTime UpdatedAt)
{
    public static FuelEntryDto FromGasBill(FuelEntry fuelEntry)
    {
        return new FuelEntryDto(fuelEntry.Id, fuelEntry.RefueledAt, fuelEntry.Liters, fuelEntry.Price, fuelEntry.Odometer, fuelEntry.FullTank, fuelEntry.CreatedAt, fuelEntry.UpdatedAt);
    }

    public FuelEntry ToGasBill(VehicleDto? vehicle = null)
    {
        var result = new FuelEntry()
        {
            Id = Id,
            RefueledAt =  RefueledAt,
            Liters = Liters,
            Price = Price,
            Odometer = Odometer,
            FullTank = FullTank,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };

        if (vehicle != null)
            result.VehicleId = vehicle.Id;
        
        return result;
    }
}