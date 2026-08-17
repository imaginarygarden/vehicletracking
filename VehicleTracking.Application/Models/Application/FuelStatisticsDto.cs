namespace VehicleTracking.Application.Models.Application;

public record FuelEntryStatisticsDto(FuelEntryDto Entry, double? Consumption);

public record FuelStatisticsDto(
    IReadOnlyCollection<FuelEntryStatisticsDto> Entries,
    double TotalLiters,
    double TotalFuelCost,
    int? TotalDistance,
    double? AverageFuelPricePerLiter,
    double? AverageConsumption,
    double? CostPerKilometer);
