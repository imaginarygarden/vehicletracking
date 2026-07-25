using System.Reflection;
using VehicleTracking.Application.Interfaces;

namespace VehicleTracking.Application.Services;

public class UtilityService : IUtilityService
{
    public object ConvertToObject(string value, Type conversionType)
    {
        try
        {
            object targetValue = value;

            if (conversionType.GetTypeInfo().IsEnum)
                targetValue = Enum.Parse(conversionType, value);
            
            return Convert.ChangeType(targetValue, conversionType);;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    public bool IsValidType(string value, Type targetType)
    {
        try
        {
            ConvertToObject(value, targetType);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}