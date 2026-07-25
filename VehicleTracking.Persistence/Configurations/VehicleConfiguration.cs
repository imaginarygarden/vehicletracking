using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicle");
        builder.HasKey(e => e.Id).HasName("Vehicle_PK");
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.LicensePlate).IsRequired().HasMaxLength(128);
        builder.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}