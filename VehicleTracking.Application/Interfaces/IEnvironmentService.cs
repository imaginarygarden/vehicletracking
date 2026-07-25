namespace VehicleTracking.Application.Interfaces;

public interface IEnvironmentService
{
    string? GetVariable(string key);
}