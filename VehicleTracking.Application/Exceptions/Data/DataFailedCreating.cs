namespace VehicleTracking.Application.Exceptions.Data;

public class DataFailedCreating : Exception
{
    public DataFailedCreating() : base("Data entity was not created.")
    {
    }
    
    public DataFailedCreating(Type type) : base($"{type.FullName} data entity was not created.")
    {
    }
    
    public DataFailedCreating(Type type, Exception? innerException) : base($"{type.FullName} data entity was not created.", innerException)
    {
    }
}