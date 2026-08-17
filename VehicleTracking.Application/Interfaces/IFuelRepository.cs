using VehicleTracking.Application.Models.Application;
using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Application.Interfaces;

public interface IFuelRepository
{
    Task<ICollection<FuelEntryDto>> GetForVehicleAsync(Guid vehicleId, SessionDto session);
    Task<bool> AddAsync(Guid vehicleId, FuelEntryDataDto data, SessionDto session);
    Task<bool> UpdateAsync(Guid vehicleId, FuelEntryDto fuelEntry, FuelEntryDataDto data, SessionDto session);
    Task<bool> RemoveAsync(Guid vehicleId, Guid fuelEntryId, SessionDto session);
}
