using VehicleTracking.Web.Resources;

namespace VehicleTracking.Web.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api");

        group.MapPost("/login", AuthResources.Login)
            .AllowAnonymous();

        group.MapPost("/register", AuthResources.Register)
            .AllowAnonymous();
        
        group.MapGet("/logout", AuthResources.Logout)
            .RequireAuthorization();
    }
}