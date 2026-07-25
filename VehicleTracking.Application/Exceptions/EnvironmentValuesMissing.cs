namespace VehicleTracking.Application.Exceptions;

public class EnvironmentValuesMissing : Exception
{
    public EnvironmentValuesMissing() : base("Required environment variable is missing.")
    {
    }
    
    public EnvironmentValuesMissing(string? name) : base($"{name} environment variable is missing.")
    {
    }
    
    public EnvironmentValuesMissing(string? name, Exception? innerException) : base($"{name} environment variable is missing.", innerException)
    {
    }
}