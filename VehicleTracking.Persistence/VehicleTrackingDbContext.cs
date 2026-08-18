using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.Interfaces;

namespace VehicleTracking.Persistence;

public class VehicleTrackingDbContext(DbContextOptions<VehicleTrackingDbContext> options, TimeProvider timeProvider) : DbContext(options)
{
    public DbSet<User> User => Set<User>();
    public DbSet<Session> Session => Set<Session>();
    public DbSet<Vehicle> Vehicle => Set<Vehicle>();
    public DbSet<FuelEntry> FuelEntry => Set<FuelEntry>();

    private void UpdateTimestamps()
    {
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        foreach (var entry in ChangeTracker.Entries<IEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Id = Guid.NewGuid();
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.UpdatedAt = utcNow;
                    
                    if (entry.Entity is ITrackedEntity activity)
                        activity.LastSeenAt = utcNow;
                    
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = utcNow;
                    break;
            }
        }
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("public");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}