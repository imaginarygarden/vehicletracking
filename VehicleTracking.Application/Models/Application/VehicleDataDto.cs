using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Models.Application;

public record VehicleDataDto(DateOnly FirstRegistration, string LicensePlate)
{
    public Vehicle ToVehicle(SessionDto? session = null)
    {
        var result = new Vehicle()
        {
            FirstRegistration = FirstRegistration,
            LicensePlate = LicensePlate,
        };

        if (session != null && Guid.TryParse(session.UserId, out var userId))
            result.UserId = userId;
        
        return result;
    }
}