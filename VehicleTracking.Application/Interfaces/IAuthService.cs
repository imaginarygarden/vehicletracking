using VehicleTracking.Application.Models;
using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Application.Interfaces;

public interface IAuthService
{
    Task<ResponseDto<UserDto>> AuthenticateAsync(LoginRequestDto loginRequest);
    
    Task<ResponseDto<UserDto>> RegisterAsync(RegisterRequestDto registerRequest);
}