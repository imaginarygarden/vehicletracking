using System.Security.Claims;
using VehicleTracking.Application.Common;
using VehicleTracking.Application.Enums;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Application.Services;

// TODO: run IP, UA against public databases and log them for future threat responses
public class Verificator(IDataStore dataStore, TimeProvider timeProvider) : IVerificator
{
    private DeploymentType _deploymentType = 
        EnvironmentUtilities.GetVariable<DeploymentType>("ASPNETCORE_ENVIRONMENT");
    private int _passwordMinStrength = 
        EnvironmentUtilities.GetVariable<int>("AUTH_PASSWORD_MIN_STRENGTH");
    
    private async Task<bool> VerifyUserAgentAsync(string? userAgent)
    {
        return true;
    }

    private async Task<bool> VerifyIpAddressAsync(string? ipAddress)
    {
        return true;
    }

    public async Task<bool> VerifySecurityInfoAsync(SecurityInformationDto securityInformationDto)
    {
        if (_deploymentType == DeploymentType.Production)
        {
            var userAgentVerification = await VerifyUserAgentAsync(securityInformationDto.UserAgent);
            var ipAddressVerification = await VerifyIpAddressAsync(securityInformationDto.IpAddress);
            
            var isVerified = userAgentVerification && ipAddressVerification;

            return isVerified;
        }

        return true;
    }

    public async Task<bool> VerifyClaimsAsync(IEnumerable<Claim> claims)
    {
        var dummyClaims = SessionDto.Dummy.Claims;
        var isMatchedOrDefault = claims?.All(i => dummyClaims.Any(j => j.Type == i.Type)) ?? false;

        return isMatchedOrDefault;
    }


    public PasswordVerificationDto VerifyPassword(string password)
    {
        if (_deploymentType == DeploymentType.Production)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            var suggestions = result.Feedback.Suggestions;
            var strength = result.Score * 20 + 20;

            return new PasswordVerificationDto(strength >= _passwordMinStrength, strength,
                string.Join(" ", suggestions));
        }

        return new PasswordVerificationDto(true, 100, "");
    }
}