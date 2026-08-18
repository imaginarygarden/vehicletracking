namespace VehicleTracking.Application.Models.Authentication;

public record PasswordVerificationDto(bool Success, int Strength, string Suggestions);