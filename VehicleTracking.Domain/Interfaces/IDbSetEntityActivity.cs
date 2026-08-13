namespace VehicleTracking.Application.Interfaces;

public interface IDbSetEntityActivity : IDbSetEntity
{
    public DateTime LastSeenAt { get; set; }
}