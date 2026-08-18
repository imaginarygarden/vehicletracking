using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Web.Extensions;

public static class HttpContextExtensions
{
    public static SecurityInformationDto GetSecurityInformation(this HttpContext context)
    {
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        
        return new SecurityInformationDto(userAgent, ipAddress);
    }
}