namespace VehicleTracking.Application.Models.Application;

public record FuelEntryStatisticsDto(FuelEntryDto Entry, decimal? Consumption);

public record FuelStatisticsDto(
    IReadOnlyCollection<FuelEntryStatisticsDto> Entries,
    decimal TotalLiters,
    decimal TotalFuelCost,
    int? TotalDistance,
    decimal? AverageFuelPricePerLiter,
    decimal? AverageConsumption,
    decimal? CostPerKilometer);
