using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using VehicleTracking.Application.Common;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Web.Extensions;

namespace VehicleTracking.Web.Utilities;

public static class AuthUtilities
{
    public static async Task OnValidatePrincipal(
        CookieValidatePrincipalContext context)
    {
        var authService =
            context.HttpContext.RequestServices
                .GetRequiredService<IAuthService>();
        
        var timeProvider =
            context.HttpContext.RequestServices
                .GetRequiredService<TimeProvider>();

        if (context.Principal?.GetSession() is { } currentSession)
        {
            var issuedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(currentSession.IssuedAt)).UtcDateTime;
            
            if (timeProvider.GetUtcNow() - issuedAt <
                TimeSpan.FromMinutes(EnvironmentUtilities.GetVariable<int>("AUTH_REFRESH_MINUTES")))
                return;
            
            var securityInformation = context.HttpContext.GetSecurityInformation();
            
            var response = await authService.RegenerateSessionAsync(
                currentSession, securityInformation);

            if (response is not null)
            {
                var identity = new ClaimsIdentity(response.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                context.ReplacePrincipal(principal);
                context.ShouldRenew = true;
                
                return;
            }
        }
        
        context.RejectPrincipal();
    }
    
    public static async Task<IResult> SignInUserAsync(HttpContext context, SessionDto session)
    {
        var identity = new ClaimsIdentity(session.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(identity));

        return Results.Ok();
    }
    
    public static async Task<IResult> SignOutUserAsync(HttpContext context, IResult result)
    {
        await context.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return result;
    }

}