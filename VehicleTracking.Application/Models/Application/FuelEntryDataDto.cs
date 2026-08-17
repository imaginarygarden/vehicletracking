using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Models.Application;

public record FuelEntryDataDto(
    DateTime RefueledAt,
    decimal Liters,
    decimal TotalPrice,
    int Odometer,
    bool FullTank)
{
    public FuelEntry ToFuelEntry(Guid vehicleId)
    {
        return new FuelEntry
        {
            VehicleId = vehicleId,
            RefueledAt = RefueledAt,
            Liters = Liters,
            TotalPrice = TotalPrice,
            Odometer = Odometer,
            FullTank = FullTank
        };
    }
}
