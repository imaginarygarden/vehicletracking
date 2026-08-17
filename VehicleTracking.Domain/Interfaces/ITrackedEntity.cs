namespace VehicleTracking.Domain.Interfaces;

public interface ITrackedEntity : IEntity
{
    public DateTime LastSeenAt { get; set; }
}