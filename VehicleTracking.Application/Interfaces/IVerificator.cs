using System.Security.Claims;
using VehicleTracking.Application.Models;
using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Application.Interfaces;

public interface IVerificator
{
    Task<bool> VerifySecurityInfoAsync(SecurityInformationDto securityInformationDto);
    Task<bool> VerifyClaimsAsync(IEnumerable<Claim>? claims);
    Task<bool> VerifyPasswordAsync(string password);
}