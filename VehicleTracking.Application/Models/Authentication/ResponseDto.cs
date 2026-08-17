using VehicleTracking.Application.Enums;

namespace VehicleTracking.Application.Models.Authentication;

public abstract record ResponseDto<T>()
{
    public sealed record SuccessDto(T Value) : ResponseDto<T>;
    public sealed record FailureDto(ResponseCode Code, string Message, Dictionary<string, string[]>? Details = null) : ResponseDto<T>;
}