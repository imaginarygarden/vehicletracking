namespace VehicleTracking.Application.Exceptions.Authentication;

public class NonUniqueCredentials : Exception
{
    public NonUniqueCredentials() : base("Email or username already exist.")
    {
    }
    
    public NonUniqueCredentials(string? field) : base($"{field} already exists.")
    {
    }
    
    public NonUniqueCredentials(string? field, Exception? innerException) : base($"{field} already exists.", innerException)
    {
    }
}