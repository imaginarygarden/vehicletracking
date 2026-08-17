using Microsoft.EntityFrameworkCore;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models.Application;
using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Services;

public class VehicleRepository(IDataStore dataStore) : IVehicleRepository
{
    public async Task<ICollection<VehicleDto>> GetForUserAsync(SessionDto session)
    {
        if (!Guid.TryParse(session.UserId, out var userId))
            return [];
        
        return await dataStore.QueryAsync<Vehicle, List<VehicleDto>>(
            query => query
                .Where(e => e.UserId == userId)
                .Select(e => VehicleDto.FromVehicle(e))
                .ToListAsync()
        );
    }

    public async Task<VehicleDto?> GetSingleForUser(Guid vehicleId, SessionDto session)
    {
        if (!Guid.TryParse(session.UserId, out var userId))
            return null;
        
        return await dataStore.QueryAsync<Vehicle, VehicleDto?>(
            query => query
                .Where(e => e.UserId == userId && e.Id == vehicleId)
                .Select(e => VehicleDto.FromVehicle(e))
                .FirstOrDefaultAsync()
        );
    }

    public async Task<bool> AddAsync(VehicleDataDto vehicleData, SessionDto session)
    {
        if (!Guid.TryParse(session.UserId, out _) || string.IsNullOrWhiteSpace(vehicleData.LicensePlate))
            return false;

        return await dataStore.AddAsync(vehicleData.ToVehicle(session)) != null;
    }

    public async Task<bool> UpdateAsync(VehicleDto vehicle, VehicleDataDto data, SessionDto session)
    {
        if (string.IsNullOrWhiteSpace(data.LicensePlate))
            return false;

        var verifiedVehicle = await GetSingleForUser(vehicle.Id, session);
        if (verifiedVehicle == null)
            return false;

        var vehicleObject = verifiedVehicle.ToVehicle(session);
        vehicleObject.FirstRegistration = data.FirstRegistration;
        vehicleObject.LicensePlate = data.LicensePlate;
        
        return await dataStore.UpdateAsync(vehicleObject) != null;
    }
    
    public async Task<bool> RemoveAsync(Guid vehicleId, SessionDto session)
    {
        var ownedVehicle = await GetSingleForUser(vehicleId, session);
        return ownedVehicle != null && await dataStore.RemoveAsync(ownedVehicle.ToVehicle()) != null;
    }
}
