using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Application.Services;

public class UserService : IUserService
{
    public async Task<UserDto?> AuthenticateAsync(LoginRequestDto loginRequest)
    {
        
    }
    
    public async Task<UserDto?> RegisterAsync(RegisterRequestDto registerRequest)
    {
        
    }
}