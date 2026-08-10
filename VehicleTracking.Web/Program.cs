using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Scalar.AspNetCore;
using VehicleTracking.Application.Common;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Services;
using VehicleTracking.Persistence;
using VehicleTracking.Web;
using VehicleTracking.Web.Components;

// Ensure ASPNETCORE_ENVIRONMENT is initialized beforehand
EnvironmentUtilities.Bootstrap();

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add db connection
builder.Services.AddDbContextFactory<VehicleTrackingDbContext>(options =>
    options.UseNpgsql(EnvironmentUtilities.GetVariable<string>("CONNECTION_STRING"), 
        e => e.MigrationsHistoryTable("__EFMigrationsHistory"))
);

// Add user-defined services
builder.Services.AddScoped<IDataStore, PostgresDataStore>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(EnvironmentUtilities.GetVariable<string>("ASPNETCORE_URLS").Split(";").Last())
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = EnvironmentUtilities.GetVariable<string>("LOGIN_PATH");
        options.AccessDeniedPath = EnvironmentUtilities.GetVariable<string>("UNAUTHORIZED_PATH");
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapEndpoints();

app.Run();