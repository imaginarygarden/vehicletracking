namespace VehicleTracking.Application.Interfaces;

public interface IUtilityService
{
    object ConvertToObject(string value, Type conversionType);
    bool IsValidType(string value, Type targetType);
}