using VehicleTracking.Application.Enums;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models.Authentication;
using VehicleTracking.Web.Extensions;
using VehicleTracking.Web.Utilities;

namespace VehicleTracking.Web.Resources;

public static class AuthResources
{
    public static async Task<IResult> Login(HttpContext context, LoginRequestDto loginRequest, IAuthService authService)
    {
        var securityInformation = context.GetSecurityInformation();
        var response = await authService.LoginAsync(loginRequest, securityInformation);
        
        return response switch
        {
            ResponseDto<SessionDto>.SuccessDto success =>
                await AuthUtilities.SignInUserAsync(context, success.Value),

            ResponseDto<SessionDto>.FailureDto failure =>
                Results.Json(failure, statusCode: (int)failure.Code),
            
            _ => Results.StatusCode(500)
        };
    }
    
    public static async Task<IResult> Register(HttpContext context, RegisterRequestDto registerRequest, IAuthService authService)
    {
        var securityInformation = context.GetSecurityInformation();
        var response = await authService.RegisterAsync(registerRequest, securityInformation);
        
        return response switch
        {
            ResponseDto<SessionDto>.SuccessDto success =>
                await AuthUtilities.SignInUserAsync(context, success.Value),

            ResponseDto<SessionDto>.FailureDto failure =>
                Results.Json(failure, statusCode: (int)failure.Code),
                
            _ => Results.StatusCode(500)
        };
    }
    
    public static async Task<IResult> Logout(HttpContext context, IAuthService authService, IVerificator verificator)
    {
        var currentSession = context.User.GetSession();

        if (currentSession == null)
        {
            var failure = new ResponseDto<SessionDto>.FailureDto(ResponseCode.InternalServerError,
                "Session claims could not be verified.");
            
            return Results.Json(failure, statusCode: (int)failure.Code);
        }
        
        var response = await authService.LogoutAsync(currentSession);
        
        return response switch
        {
            ResponseDto<SessionDto>.SuccessDto _ =>
                await AuthUtilities.SignOutUserAsync(context, Results.Ok()),

            ResponseDto<SessionDto>.FailureDto failure =>
                await AuthUtilities.SignOutUserAsync(context, Results.Json(failure, statusCode: (int)failure.Code)),
                
            _ => Results.StatusCode(500)
        };
    }
}