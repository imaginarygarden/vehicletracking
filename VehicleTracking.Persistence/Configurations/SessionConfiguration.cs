using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Application.Common;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Session");
        builder.HasKey(e => e.Id).HasName("Session_PK");
        builder.Property(e => e.UserAgent).IsRequired().HasMaxLength(EnvironmentUtilities.GetVariable<int>("CREDENTIALS_MAX_LENGTH"));
        builder.Property(e => e.IpAddress).IsRequired().HasMaxLength(EnvironmentUtilities.GetVariable<int>("CREDENTIALS_MAX_LENGTH"));
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
        builder.Property(e => e.LastSeenAt).IsRequired();
        
        builder.HasOne(e => e.User)
            .WithMany(e => e.Sessions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}