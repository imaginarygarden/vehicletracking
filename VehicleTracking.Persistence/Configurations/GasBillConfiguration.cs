using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence.Configurations;

public class GasBillConfiguration : IEntityTypeConfiguration<GasBill>
{
    public void Configure(EntityTypeBuilder<GasBill> builder)
    {
        builder.ToTable("GasBill");
        builder.HasKey(e => e.Id).HasName("GasBill_PK");
        builder.Property(e => e.Liters).IsRequired();
        builder.Property(e => e.Price).IsRequired();
        builder.Property(e => e.Mileage).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasOne(e => e.Vehicle)
            .WithMany(e => e.GasBills)
            .HasForeignKey(e => e.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}