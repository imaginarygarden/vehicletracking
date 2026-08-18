using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Application.Interfaces;

public interface IVerificator
{
    Task<bool> VerifySecurityInfoAsync(SecurityInformationDto securityInformationDto);
    public PasswordVerificationDto VerifyPassword(string password);
}