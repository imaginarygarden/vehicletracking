namespace VehicleTracking.Application.Exceptions.Authentication;

public class InvalidCredentials : Exception
{
    public InvalidCredentials() : base("Username or password is incorrect.")
    {
    }
    
    public InvalidCredentials(string? message) : base(message)
    {
    }
    
    public InvalidCredentials(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}