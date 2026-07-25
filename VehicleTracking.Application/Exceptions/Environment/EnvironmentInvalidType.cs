namespace VehicleTracking.Application.Exceptions.Environment;

public class EnvironmentInvalidType : Exception
{
    public EnvironmentInvalidType() : base("Required environment variable has value of invalid type.")
    {
    }
    
    public EnvironmentInvalidType(string? name, Type requiredType) : base($"{name} environment variable has value of invalid type. Required type is {requiredType}.")
    {
    }
    
    public EnvironmentInvalidType(string? name, Type requiredType,  Exception? innerException) : base($"{name} environment variable has value of invalid type. Required type is {requiredType}.", innerException)
    {
    }
}