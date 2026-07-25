namespace VehicleTracking.Application.Exceptions.Environment;

public class EnvironmentInvalidRequestedType : Exception
{
    public EnvironmentInvalidRequestedType() : base("Required environment variable was requested using invalid type.")
    {
    }
    
    public EnvironmentInvalidRequestedType(string? name) : base($"{name} environment variable was requested using invalid type.")
    {
    }
    
    public EnvironmentInvalidRequestedType(string? name, Exception? innerException) : base($"{name} environment variable was requested using invalid type.", innerException)
    {
    }
}