using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Models.Application;

public record FuelEntryDto(
    Guid Id, 
    DateTime RefueledAt,
    decimal Liters, 
    decimal TotalPrice,
    int Odometer, 
    bool FullTank,
    DateTime CreatedAt, 
    DateTime UpdatedAt)
{
    public static FuelEntryDto FromFuelEntry(FuelEntry fuelEntry)
    {
        return new FuelEntryDto(fuelEntry.Id, fuelEntry.RefueledAt, fuelEntry.Liters, fuelEntry.TotalPrice, fuelEntry.Odometer, fuelEntry.FullTank, fuelEntry.CreatedAt, fuelEntry.UpdatedAt);
    }

    public FuelEntry ToFuelEntry(VehicleDto? vehicle = null)
    {
        var result = new FuelEntry()
        {
            Id = Id,
            RefueledAt =  RefueledAt,
            Liters = Liters,
            TotalPrice = TotalPrice,
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