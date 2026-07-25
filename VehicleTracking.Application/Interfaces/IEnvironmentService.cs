namespace VehicleTracking.Application.Interfaces;

public interface IEnvironmentService
{
    T GetVariable<T>(string key);
}