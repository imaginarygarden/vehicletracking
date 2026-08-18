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
        builder.HasIndex(e => e.Username).IsUnique();
        builder.HasIndex(e => e.Email).IsUnique();
        
        builder.Property(e => e.Username).IsRequired().HasMaxLength(EnvironmentUtilities.GetVariable<int>("CREDENTIALS_MAX_LENGTH"));
        builder.Property(e => e.Password).IsRequired().HasMaxLength(EnvironmentUtilities.GetVariable<int>("CREDENTIALS_MAX_LENGTH"));
        builder.Property(e => e.Email).IsRequired().HasMaxLength(EnvironmentUtilities.GetVariable<int>("CREDENTIALS_MAX_LENGTH"));
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
        builder.Property(e => e.LastSeenAt).IsRequired();

        // Enum conversion
        builder
            .Property(e => e.Role)
            .HasConversion(
                e => e.ToString(), 
                e => (UserRole)Enum.Parse(typeof(UserRole), e)
            );
    }
}