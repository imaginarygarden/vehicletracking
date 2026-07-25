namespace VehicleTracking.Application.Exceptions;

public class EnvironmentValuesMissing : Exception
{
    public EnvironmentValuesMissing() : base("Required environment variables are missing.")
    {
    }
    
    public EnvironmentValuesMissing(string? message) : base(message)
    {
    }
    
    public EnvironmentValuesMissing(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}