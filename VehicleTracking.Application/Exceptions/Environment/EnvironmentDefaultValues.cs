namespace VehicleTracking.Application.Exceptions.Environment;

public class EnvironmentDefaultValues : Exception
{
    public EnvironmentDefaultValues() : base("Default value was not changed for production.")
    {
    }
    
    public EnvironmentDefaultValues(string? name) : base($"{name} was not changed for production.")
    {
    }
    
    public EnvironmentDefaultValues(string? name, Exception? innerException) : base($"{name} was not changed for production.", innerException)
    {
    }
}