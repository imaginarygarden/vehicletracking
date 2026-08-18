using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Application.Common;

namespace VehicleTracking.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicle");
        builder.HasKey(e => e.Id).HasName("Vehicle_PK");
        builder.Property(e => e.LicensePlate).IsRequired().HasMaxLength(EnvironmentUtilities.GetVariable<int>("MISC_MAX_LENGTH"));
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
        
        builder.HasOne(e => e.User)
            .WithMany(e => e.Vehicles)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}