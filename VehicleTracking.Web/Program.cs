using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Services;
using VehicleTracking.Persistence;
using VehicleTracking.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add user-defined services
builder.Services.AddScoped<IUtilityService, UtilityService>();
builder.Services.AddScoped<IEnvironmentService, EnvironmentService>();

// Add db connection
builder.Services.AddDbContextFactory<VehicleTrackingDbContext>((provider, options) =>
{
    var environmentService = provider.GetRequiredService<IEnvironmentService>();
    options.UseNpgsql(environmentService.GetVariable<string>("POSTGRES_DB"));
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();