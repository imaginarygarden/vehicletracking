using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models.Application;

namespace VehicleTracking.Application.Services;

public class FuelCalculationService : IFuelCalculationService
{
    public FuelStatisticsDto Calculate(IEnumerable<FuelEntryDto> fuelEntries)
    {
        var orderedEntries = fuelEntries
            .OrderBy(e => e.RefueledAt)
            .ThenBy(e => e.CreatedAt)
            .ToList();

        var totalLiters = orderedEntries.Sum(e => e.Liters);
        var totalFuelCost = orderedEntries.Sum(e => e.TotalPrice);
        var entryStatistics = new List<FuelEntryStatisticsDto>(orderedEntries.Count);

        FuelEntryDto? previousFullTank = null;
        decimal litersSincePreviousFullTank = 0;
        decimal consumptionLiters = 0;
        int consumptionDistance = 0;

        foreach (var entry in orderedEntries)
        {
            decimal? consumption = null;

            if (previousFullTank != null)
                litersSincePreviousFullTank += entry.Liters;

            if (entry.FullTank)
            {
                if (previousFullTank != null)
                {
                    var distance = entry.Odometer - previousFullTank.Odometer;
                    if (distance > 0 && litersSincePreviousFullTank > 0)
                    {
                        consumption = litersSincePreviousFullTank / distance * 100;
                        consumptionLiters += litersSincePreviousFullTank;
                        consumptionDistance += distance;
                    }
                }

                previousFullTank = entry;
                litersSincePreviousFullTank = 0;
            }

            entryStatistics.Add(new FuelEntryStatisticsDto(entry, consumption));
        }

        int? totalDistance = null;
        if (orderedEntries.Count > 1)
        {
            var distance = orderedEntries[^1].Odometer - orderedEntries[0].Odometer;
            if (distance > 0)
                totalDistance = distance;
        }

        decimal? averageFuelPrice = totalLiters > 0 ? totalFuelCost / totalLiters : null;
        decimal? averageConsumption = consumptionDistance > 0
            ? consumptionLiters / consumptionDistance * 100
            : null;
        decimal? costPerKilometer = totalDistance > 0 ? totalFuelCost / totalDistance.Value : null;

        return new FuelStatisticsDto(
            entryStatistics
                .OrderByDescending(e => e.Entry.RefueledAt)
                .ThenByDescending(e => e.Entry.CreatedAt)
                .ToList(),
            totalLiters,
            totalFuelCost,
            totalDistance,
            averageFuelPrice,
            averageConsumption,
            costPerKilometer);
    }
}
