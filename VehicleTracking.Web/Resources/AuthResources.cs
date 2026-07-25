using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models;
using VehicleTracking.Application.Models.Authentication;

namespace VehicleTracking.Web.Resources;

public static class AuthResources
{
    public static async Task<IResult> Login(HttpContext context, LoginRequestDto loginRequest, IAuthService authService)
    {
        var response = await authService.AuthenticateAsync(loginRequest);

        return response switch
        {
            ResponseDto<UserDto>.SuccessDto successDto =>
                await SignInUserAsync(context, successDto.Value),

            ResponseDto<UserDto>.FailureDto failureDto =>
                Results.StatusCode((int)failureDto.Code)
        };
    }
    
    public static async Task<IResult> Register(HttpContext context, RegisterRequestDto registerRequest, IAuthService authService)
    {
        var response = await authService.RegisterAsync(registerRequest);
        
        return response switch
        {
            ResponseDto<UserDto>.SuccessDto successDto =>
                Results.Ok(successDto.Value),

            ResponseDto<UserDto>.FailureDto failureDto =>
                Results.StatusCode((int)failureDto.Code)
        };
    }
    
    private static async Task<IResult> SignInUserAsync(HttpContext context, UserDto user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(identity));

        return Results.Ok();
    }
}