using Microsoft.EntityFrameworkCore;
using VehicleTracking.Application.Common;
using VehicleTracking.Application.Enums;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.Enums;

namespace VehicleTracking.Application.Services;

public class AuthService(IDataStore dataStore, IVerificator verificator, TimeProvider timeProvider) : IAuthService
{
    public async Task<ResponseDto<SessionDto>> LoginAsync(LoginRequestDto loginRequest, SecurityInformationDto securityInformation)
    {
        var isVerified = await verificator.VerifySecurityInfoAsync(securityInformation);
        
        if (!isVerified)
            return new ResponseDto<SessionDto>.FailureDto(
                Code: ResponseCode.Unauthorized,
                Message: "Invalid login attempt."
            );
        
        var user = await dataStore.QueryAsync<User, User?>(
            query => query
            .FirstOrDefaultAsync(e => e.Username == loginRequest.Username)
        );

        if (user is null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            return new ResponseDto<SessionDto>.FailureDto(
                Code: ResponseCode.Unauthorized,
                Message: "Invalid login attempt."
            );

        var session = await GenerateSessionAsync(user, securityInformation);
        
        if (session is null)
            return new ResponseDto<SessionDto>.FailureDto(
                Code: ResponseCode.InternalServerError,
                Message: "Could not assign session."
            );

        return new ResponseDto<SessionDto>.SuccessDto(session);
    }
    
    public async Task<ResponseDto<SessionDto>> RegisterAsync(RegisterRequestDto registerRequest, SecurityInformationDto securityInformation)
    {
        var isVerified = await verificator.VerifySecurityInfoAsync(securityInformation);
        
        if (!isVerified)
            return new ResponseDto<SessionDto>.FailureDto(
                Code: ResponseCode.Unauthorized,
                Message: "Invalid registration attempt."
            );
        
        var passwordVerification = verificator.VerifyPassword(registerRequest.Password);
        
        if (!passwordVerification.Success)
            return new ResponseDto<SessionDto>.FailureDto(
                Code: ResponseCode.BadRequest,
                Message: passwordVerification.Suggestions
            );
        
        var isEmailUsed = await dataStore.QueryAsync<User, bool>(query => query
            .AnyAsync(e => e.Email == registerRequest.Email)
        );
        
        if (isEmailUsed)
            return new ResponseDto<SessionDto>.FailureDto(
                Code: ResponseCode.Unauthorized,
                Message: "Email is already in use."
            );

        var isUsernameUsed = await dataStore.QueryAsync<User, bool>(query => query
            .AnyAsync(e => e.Username == registerRequest.Username)
        );
        
        if (isUsernameUsed)
            return new ResponseDto<SessionDto>.FailureDto(
                Code: ResponseCode.Unauthorized,
                Message: "Username is already in use."
            );
        
        var user = await dataStore.AddAsync(
            new User()
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(
                    registerRequest.Password, 
                    EnvironmentUtilities.GetVariable<int>("BCRYPT_FACTOR"),
                    EnvironmentUtilities.GetVariable<bool>("BCRYPT_ENHANCED")
                ),
                Role = EnvironmentUtilities.GetVariable<UserRole>("STANDARD_ROLE"),
            }
        );

        if (user is null)
            return new ResponseDto<SessionDto>.FailureDto(
                Code: ResponseCode.InternalServerError,
                Message: "Could not register user."
            );
        
        var session = await GenerateSessionAsync(user, securityInformation);
        
        if (session is null)
            return new ResponseDto<SessionDto>.FailureDto(
                Code: ResponseCode.InternalServerError,
                Message: "Could not assign session."
            );

        return new ResponseDto<SessionDto>.SuccessDto(session);
    }
    
    public async Task<ResponseDto<SessionDto>> LogoutAsync(SessionDto session)
    {
        var sessionObject = await dataStore.QueryAsync<Session, Session?>(query => query
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == Guid.Parse(session.SessionId))
        );

        if (sessionObject is null)
            return new ResponseDto<SessionDto>.FailureDto(
                Code: ResponseCode.Unauthorized,
                Message: "Session is incorrect."
            );
        
        await dataStore.RemoveAsync(sessionObject);
        
        return new ResponseDto<SessionDto>.SuccessDto(session);
    }

    public async Task<SessionDto?> GenerateSessionAsync(User user, SecurityInformationDto securityInformation)
    {
        var session = await dataStore.AddAsync(new Session()
        {
            UserId = user.Id,
            UserAgent = securityInformation.UserAgent!,
            IpAddress = securityInformation.IpAddress!,
        });
        
        if (session is null || await dataStore.QueryAsync<Session, Session?>(
                query => query
                    .Include(e => e.User)
                    .FirstOrDefaultAsync(e => e.Id == session.Id)) 
                is not { } sessionObject
            )
            return null;

        return SessionDto.FromSession(sessionObject, timeProvider);
    }

    public async Task<SessionDto?> RegenerateSessionAsync(SessionDto session, SecurityInformationDto securityInformation)
    {
        var isVerified = await verificator.VerifySecurityInfoAsync(securityInformation);

        if (!isVerified)
            return null;
        
        var sessionObject = await dataStore.QueryAsync<Session, Session?>(
            query => query
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == Guid.Parse(session.SessionId))
        );

        if (sessionObject is null)
            return null;
        
        return SessionDto.FromSession(sessionObject, timeProvider);
    }
}