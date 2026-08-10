using Microsoft.AspNetCore.Http.HttpResults;
using VehicleTracking.Web.Resources;

namespace VehicleTracking.Web;

public static class EndpointRouteBuilderExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api");

        group.MapPost("/login", AuthResources.Login)
            .AllowAnonymous();

        group.MapPost("/register", AuthResources.Register)
            .AllowAnonymous();
        
        group.MapPost("/logout", AuthResources.Logout)
            .RequireAuthorization();
    }
}