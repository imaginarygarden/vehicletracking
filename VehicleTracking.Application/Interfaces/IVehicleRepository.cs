using VehicleTracking.Application.Models.Application;
using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Application.Interfaces;

public interface IVehicleRepository
{
    Task<ICollection<VehicleDto>> GetForUserAsync(SessionDto session);
    Task<VehicleDto?> GetSingleForUser(Guid vehicleId, SessionDto session);
    Task<bool> AddAsync(VehicleDataDto vehicleData, SessionDto session);
    Task<bool> UpdateAsync(VehicleDto vehicle, VehicleDataDto data, SessionDto session);
    Task<bool> RemoveAsync(Guid vehicleId, SessionDto session);
}
