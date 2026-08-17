using VehicleTracking.Application.Models.Application;

namespace VehicleTracking.Application.Interfaces;

public interface IFuelCalculationService
{
    FuelStatisticsDto Calculate(IEnumerable<FuelEntryDto> fuelEntries);
}
