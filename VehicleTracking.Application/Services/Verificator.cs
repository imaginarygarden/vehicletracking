using System.Security.Claims;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Application.Services;

// TODO: run IP, UA against public databases and log them for future threat responses
public class Verificator(IDataStore dataStore, TimeProvider timeProvider) : IVerificator
{
    private async Task<bool> VerifyUserAgentAsync(string? userAgent)
    {
        await Task.CompletedTask;
        return true;
    }

    private async Task<bool> VerifyIpAddressAsync(string? ipAddress)
    {
        await Task.CompletedTask;
        return true;
    }

    public async Task<bool> VerifySecurityInfoAsync(SecurityInformationDto securityInformationDto)
    {
        var userAgentVerification = await VerifyUserAgentAsync(securityInformationDto.UserAgent);
        var ipAddressVerification = await VerifyIpAddressAsync(securityInformationDto.IpAddress);
        
        var isVerified = userAgentVerification && ipAddressVerification;

        return isVerified;
    }

    public async Task<bool> VerifyClaimsAsync(IEnumerable<Claim>? claims)
    {
        var dummyClaims = SessionDto.Dummy.Claims;
        var isMatchedOrDefault = claims?.All(i => dummyClaims.Any(j => j.Type == i.Type)) ?? false;

        return isMatchedOrDefault;
    }

    public async Task<bool> VerifyPasswordAsync(string password)
    {
        await Task.CompletedTask;
        return true;
    }
}