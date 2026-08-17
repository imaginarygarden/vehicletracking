using System.Numerics;
using Microsoft.EntityFrameworkCore;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models.Application;
using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Services;

public class FuelRepository(IDataStore dataStore) : IFuelRepository
{
    public async Task<ICollection<FuelEntryDto>> GetForVehicleAsync(Guid vehicleId, SessionDto session)
    {
        if (!Guid.TryParse(session.UserId, out var userId))
            return [];

        return await dataStore.QueryAsync<FuelEntry, List<FuelEntryDto>>(
            query => query
                .Where(e => e.VehicleId == vehicleId && e.Vehicle.UserId == userId)
                .OrderByDescending(e => e.RefueledAt)
                .ThenByDescending(e => e.CreatedAt)
                .Select(e => FuelEntryDto.FromFuelEntry(e))
                .ToListAsync());
    }

    public async Task<bool> AddAsync(Guid vehicleId, FuelEntryDataDto data, SessionDto session)
    {
        if (!IsValid(data) ||
            !Guid.TryParse(session.UserId, out var userId) ||
            !await OwnsVehicleAsync(vehicleId, userId))
            return false;

        return await dataStore.AddAsync(data.ToFuelEntry(vehicleId)) != null;
    }

    public async Task<bool> UpdateAsync(
        Guid vehicleId,
        FuelEntryDto fuelEntry,
        FuelEntryDataDto data,
        SessionDto session)
    {
        if (!IsValid(data) || !Guid.TryParse(session.UserId, out var userId))
            return false;

        var ownedEntry = await GetOwnedFuelEntryAsync(vehicleId, fuelEntry.Id, userId);
        if (ownedEntry == null)
            return false;

        ownedEntry.RefueledAt = data.RefueledAt;
        ownedEntry.Liters = data.Liters;
        ownedEntry.TotalPrice = data.TotalPrice;
        ownedEntry.Odometer = data.Odometer;
        ownedEntry.FullTank = data.FullTank;

        return await dataStore.UpdateAsync(ownedEntry) != null;
    }

    public async Task<bool> RemoveAsync(Guid vehicleId, Guid fuelEntryId, SessionDto session)
    {
        if (!Guid.TryParse(session.UserId, out var userId))
            return false;

        var ownedEntry = await GetOwnedFuelEntryAsync(vehicleId, fuelEntryId, userId);
        return ownedEntry != null && await dataStore.RemoveAsync(ownedEntry) != null;
    }

    private async Task<bool> OwnsVehicleAsync(Guid vehicleId, Guid userId)
    {
        return await dataStore.QueryAsync<Vehicle, bool>(
            query => query.AnyAsync(e => e.Id == vehicleId && e.UserId == userId));
    }

    private async Task<FuelEntry?> GetOwnedFuelEntryAsync(Guid vehicleId, Guid fuelEntryId, Guid userId)
    {
        return await dataStore.QueryAsync<FuelEntry, FuelEntry?>(
            query => query.FirstOrDefaultAsync(e =>
                e.Id == fuelEntryId &&
                e.VehicleId == vehicleId &&
                e.Vehicle.UserId == userId));
    }

    private static bool IsValid(FuelEntryDataDto data)
    {
        double.IsNaN(2);
        return data.RefueledAt != default &&
               data.Odometer is >= 0 and <= 10000000 &&
               data.Liters is > 0 and <= 10000 &&
               data.TotalPrice is >= 0 and <= 1000000
               ;
    }
}
