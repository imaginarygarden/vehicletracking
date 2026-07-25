namespace VehicleTracking.Application.Exceptions;

public class EnvironmentInvalidType : Exception
{
    public EnvironmentInvalidType() : base("Required environment variable has value of invalid type.")
    {
    }
    
    public EnvironmentInvalidType(string? name) : base($"{name} environment variable has value of invalid type.")
    {
    }
    
    public EnvironmentInvalidType(string? name, Exception? innerException) : base($"{name} environment variable has value of invalid type.", innerException)
    {
    }
}