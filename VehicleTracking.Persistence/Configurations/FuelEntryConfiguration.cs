using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence.Configurations;

public class FuelEntryConfiguration : IEntityTypeConfiguration<FuelEntry>
{
    public void Configure(EntityTypeBuilder<FuelEntry> builder)
    {
        builder.ToTable("FuelEntry");
        builder.HasKey(e => e.Id).HasName("FuelEntry_PK");
        builder.Property(e => e.RefueledAt).IsRequired();
        builder.Property(e => e.Liters).HasPrecision(10, 3).IsRequired();
        builder.Property(e => e.TotalPrice).HasPrecision(12, 3).IsRequired();
        builder.Property(e => e.Odometer).IsRequired();
        builder.Property(e => e.FullTank).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasOne(e => e.Vehicle)
            .WithMany(e => e.FuelEntries)
            .HasForeignKey(e => e.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}