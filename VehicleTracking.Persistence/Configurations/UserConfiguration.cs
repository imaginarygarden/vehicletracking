using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Application.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.Enums;

namespace VehicleTracking.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(e => e.Id).HasName("User_PK");
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Username).IsRequired().HasMaxLength(EnvironmentUtilities.GetVariable<int>("CREDENTIALS_MAX_LENGTH"));
        builder.Property(e => e.Password).IsRequired().HasMaxLength(EnvironmentUtilities.GetVariable<int>("CREDENTIALS_MAX_LENGTH"));
        builder.Property(e => e.Email).IsRequired().HasMaxLength(EnvironmentUtilities.GetVariable<int>("CREDENTIALS_MAX_LENGTH"));
        builder.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Enum conversion
        builder
            .Property(e => e.Role)
            .HasConversion(
                e => e.ToString(), 
                e => (UserRole)Enum.Parse(typeof(UserRole), e)
            );
    }
}