using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using VehicleTracking.Domain.Enums;

namespace VehicleTracking.Web.Extensions;

public static class AuthorizationOptionsExtensions
{
    
    public static void AddPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy("NotBannedAndAuthorized", policy =>
        {
            policy.RequireAssertion(context =>
            {
                var role = context.User.GetUserRole();

                return role != null && role != UserRole.Banned;
            });
        });
    }
}