namespace VehicleTracking.Application.Exceptions;

public class EnvironmentDefaultValues : Exception
{
    public EnvironmentDefaultValues() : base("Default values were not changed for production.")
    {
    }
    
    public EnvironmentDefaultValues(string? message) : base(message)
    {
    }
    
    public EnvironmentDefaultValues(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}