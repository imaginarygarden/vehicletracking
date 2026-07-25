namespace VehicleTracking.Application.Exceptions;

public class EnvironmentInvalidRequestedKey : Exception
{
    public EnvironmentInvalidRequestedKey() : base("Requested environment variable does not exist in the registry.")
    {
    }
    
    public EnvironmentInvalidRequestedKey(string? name) : base($"{name} environment variable does not exist in the registry.")
    {
    }
    
    public EnvironmentInvalidRequestedKey(string? name, Exception? innerException) : base($"{name} environment variable does not exist in the registry.", innerException)
    {
    }
}