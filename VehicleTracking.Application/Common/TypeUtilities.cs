using System.Reflection;

namespace VehicleTracking.Application.Common;

public static class TypeUtilities
{
    public static object Parse(string value, Type conversionType)
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
    
    public static bool Validate(string value, Type targetType)
    {
        try
        {
            Parse(value, targetType);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
}