using VehicleTracking.Application.Enums;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models;
using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Web.Extensions;

namespace VehicleTracking.Web.Resources;

public static class AuthResources
{
    public static async Task<IResult> Login(HttpContext context, LoginRequestDto loginRequest, IAuthService authService)
    {
        var securityInformation = SecurityInformationDto.FromContext(context);
        var response = await authService.LoginAsync(loginRequest, securityInformation);
        
        return response switch
        {
            ResponseDto<SessionDto>.SuccessDto success =>
                await AuthExtensions.SignInUserAsync(context, success.Value),

            ResponseDto<SessionDto>.FailureDto failure =>
                Results.Json(failure, statusCode: (int)failure.Code),
            
            _ => Results.StatusCode(500)
        };
    }
    
    public static async Task<IResult> Register(HttpContext context, RegisterRequestDto registerRequest, IAuthService authService)
    {
        var securityInformation = SecurityInformationDto.FromContext(context);
        var response = await authService.RegisterAsync(registerRequest, securityInformation);
        
        return response switch
        {
            ResponseDto<SessionDto>.SuccessDto success =>
                await AuthExtensions.SignInUserAsync(context, success.Value),

            ResponseDto<SessionDto>.FailureDto failure =>
                Results.Json(failure, statusCode: (int)failure.Code),
                
            _ => Results.StatusCode(500)
        };
    }
    
    public static async Task<IResult> Logout(HttpContext context, IAuthService authService, IVerificator verificator)
    {
        var isVerified = await verificator.VerifyClaimsAsync(context.User.Claims);

        if (!isVerified)
        {
            var failure = new ResponseDto<SessionDto>.FailureDto(ResponseCode.InternalServerError,
                "Session claims could not be verified.");
            
            return Results.Json(failure, statusCode: (int)failure.Code);
        }
        
        var session = SessionDto.FromClaims(context.User.Claims);
        var response = await authService.LogoutAsync(session);
        
        return response switch
        {
            ResponseDto<SessionDto>.SuccessDto success =>
                await AuthExtensions.SignOutUserAsync(context, success.Value),

            ResponseDto<SessionDto>.FailureDto failure =>
                Results.Json(failure, statusCode: (int)failure.Code),
                
            _ => Results.StatusCode(500)
        };
    }
}