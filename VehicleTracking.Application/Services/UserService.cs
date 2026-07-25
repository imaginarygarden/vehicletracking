using VehicleTracking.Application.Common;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.Enums;

namespace VehicleTracking.Application.Services;

public class UserService(IDataStore dataStore) : IUserService
{
    public async Task<UserDto?> AuthenticateAsync(LoginRequestDto loginRequest)
    {
        var users = await dataStore.GetAsync<User>();
        var user = users.FirstOrDefault(e => e.Username == loginRequest.Username && e.Password == loginRequest.Password);

        if (user == null)
            return null;

        return new UserDto(user.Id, user.Username, user.Role.ToString());
    }
    
    public async Task<UserDto?> RegisterAsync(RegisterRequestDto registerRequest)
    {
        // add appropriate data integrity checks
        var user = await dataStore.AddAsync<User>(
            new User()
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                Password = registerRequest.Password,
                Role = EnvironmentUtilities.GetVariable<UserRole>("STANDARD_ROLE")
            }
        );

        return new UserDto(user.Id, user.Username, user.Role.ToString());
    }
}