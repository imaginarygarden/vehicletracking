namespace VehicleTracking.Application.Exceptions;

public class EnvironmentInvalidKey : Exception
{
    public EnvironmentInvalidKey() : base("Requested environment variable does not exist in the registry.")
    {
    }
    
    public EnvironmentInvalidKey(string? name) : base($"{name} environment variable does not exist in the registry.")
    {
    }
    
    public EnvironmentInvalidKey(string? name, Exception? innerException) : base($"{name} environment variable does not exist in the registry.", innerException)
    {
    }
}