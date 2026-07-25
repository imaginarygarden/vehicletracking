using VehicleTracking.Application.Common;
using VehicleTracking.Application.Enums;
using VehicleTracking.Application.Exceptions.Authentication;
using VehicleTracking.Application.Exceptions.Data;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models;
using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.Enums;

namespace VehicleTracking.Application.Services;

public class AuthService(IDataStore dataStore) : IAuthService
{
    public async Task<ResponseDto<UserDto>> AuthenticateAsync(LoginRequestDto loginRequest)
    {
        var users = await dataStore.GetAsync<User>();
        var user = users.FirstOrDefault(e => e.Username == loginRequest.Username && e.Password == loginRequest.Password);

        if (user is null)
            return new ResponseDto<UserDto>.FailureDto(
                Code: ResponseCode.Unauthorized,
                Message: "Username or password is incorrect"
            );

        return new ResponseDto<UserDto>.SuccessDto(
            new UserDto(user.Id, user.Username, user.Role.ToString())
        );
    }
    
    public async Task<ResponseDto<UserDto>> RegisterAsync(RegisterRequestDto registerRequest)
    {
        var users = await dataStore.GetAsync<User>();
        
        if (users.Any(e => e.Email == registerRequest.Email))
            return new ResponseDto<UserDto>.FailureDto(
                Code: ResponseCode.Unauthorized,
                Message: "Email is already in use."
            );

        if (users.Any(e => e.Username == registerRequest.Username))
            return new ResponseDto<UserDto>.FailureDto(
                Code: ResponseCode.Unauthorized,
                Message: "Username is already in use."
            );
        
        var user = await dataStore.AddAsync(
            new User()
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                Password = registerRequest.Password,
                Role = EnvironmentUtilities.GetVariable<UserRole>("STANDARD_ROLE")
            }
        );

        if (user is null)
            return new ResponseDto<UserDto>.FailureDto(
                Code: ResponseCode.InternalServerError,
                Message: "Could not register user."
            );

        return new ResponseDto<UserDto>.SuccessDto(
            new UserDto(user.Id, user.Username, user.Role.ToString())
        );
    }
}