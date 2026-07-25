using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> AuthenticateAsync(LoginRequestDto loginRequest);
}