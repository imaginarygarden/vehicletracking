using Microsoft.AspNetCore.Http;

namespace VehicleTracking.Application.Models.Authentication;

public record SecurityInformationDto(string? UserAgent, string? IpAddress)
{
    public static SecurityInformationDto FromContext(HttpContext context)
    {
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        
        return new SecurityInformationDto(userAgent, ipAddress);
    }
}