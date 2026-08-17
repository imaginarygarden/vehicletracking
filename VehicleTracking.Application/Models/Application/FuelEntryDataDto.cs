using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Models.Application;

public record FuelEntryDataDto(
    DateTime RefueledAt,
    int Liters,
    double Price,
    int Odometer,
    bool FullTank)
{
    public FuelEntry ToGasBill(Guid vehicleId)
    {
        return new FuelEntry
        {
            VehicleId = vehicleId,
            RefueledAt = RefueledAt,
            Liters = Liters,
            Price = Price,
            Odometer = Odometer,
            FullTank = FullTank
        };
    }
}
