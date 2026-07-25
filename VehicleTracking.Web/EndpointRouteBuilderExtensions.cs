using Microsoft.AspNetCore.Http.HttpResults;

namespace VehicleTracking.Web;

public static class EndpointRouteBuilderExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api");

        // group.MapGet("/test", async Task<Results<NotFound>> () => { return await NotFound(); });
    }
}