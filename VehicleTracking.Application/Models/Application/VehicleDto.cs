using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Models.Application;

public record VehicleDto(
    Guid Id,
    DateOnly FirstRegistration,
    string LicensePlate,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static VehicleDto FromVehicle(Vehicle vehicle)
    {
        return new VehicleDto(vehicle.Id, vehicle.FirstRegistration, vehicle.LicensePlate, vehicle.CreatedAt, vehicle.UpdatedAt);
    }

    public Vehicle ToVehicle(SessionDto? session = null)
    {
        var result = new Vehicle()
        {
            Id = Id,
            FirstRegistration = FirstRegistration,
            LicensePlate = LicensePlate,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };

        if (session != null && Guid.TryParse(session.UserId, out var userId))
            result.UserId = userId;
        
        return result;
    }
}