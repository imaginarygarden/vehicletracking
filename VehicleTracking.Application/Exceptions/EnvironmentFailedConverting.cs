namespace VehicleTracking.Application.Exceptions;

public class EnvironmentFailedConverting : Exception
{
    public EnvironmentFailedConverting() : base("Required environment variable failed to convert.")
    {
    }
    
    public EnvironmentFailedConverting(string? name) : base($"{name} environment variable failed to convert.")
    {
    }
    
    public EnvironmentFailedConverting(string? name, Exception? innerException) : base($"{name} environment variable failed to convert.", innerException)
    {
    }
}