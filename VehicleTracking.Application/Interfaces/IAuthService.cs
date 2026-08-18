using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Interfaces;

public interface IAuthService
{
    Task<ResponseDto<SessionDto>> LoginAsync(LoginRequestDto loginRequest, SecurityInformationDto securityInformation);
    
    Task<ResponseDto<SessionDto>> RegisterAsync(RegisterRequestDto registerRequest, SecurityInformationDto securityInformation);
    
    Task<ResponseDto<SessionDto>> LogoutAsync(SessionDto session);

    Task<SessionDto?> GenerateSessionAsync(User user, SecurityInformationDto securityInformation);
    
    Task<SessionDto?> RegenerateSessionAsync(SessionDto session, SecurityInformationDto securityInformation);
}