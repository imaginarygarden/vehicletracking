using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models;
using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Web.Resources;

public static class AuthResources
{
    public static async Task<IResult> Login(HttpContext context, LoginRequestDto loginRequest, IUserService userService)
    {
        var user = await userService.AuthenticateAsync(loginRequest);

        if (user is null)
            return Results.Unauthorized();

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return Results.Ok();
    }
}