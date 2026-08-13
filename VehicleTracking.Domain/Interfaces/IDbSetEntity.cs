namespace VehicleTracking.Application.Interfaces;

public interface IDbSetEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}