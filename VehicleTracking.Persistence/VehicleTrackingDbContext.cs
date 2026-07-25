using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence;

public class VehicleTrackingDbContext : DbContext
{
    public DbSet<Vehicle> Vehicle => Set<Vehicle>();
    public DbSet<GasBill> GasBill => Set<GasBill>();
    
    protected VehicleTrackingDbContext() {}
    public VehicleTrackingDbContext(DbContextOptions options) : base(options) {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}