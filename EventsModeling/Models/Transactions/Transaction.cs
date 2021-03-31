using System;
using EventsModeling.Services;
using EventsModeling.Settings;

namespace EventsModeling.Models.Transactions
{
    public class Transaction
    {
        public string Type { get; }
        public int RequiredCoresCount { get; }
        public double RequiredRamCount { get; }
        public double CalculationTime { get; }
        public DateTime CreatedAt { get; }

        public Transaction(string type, TransactionSettings settings, int requiredCoresCount)
        {
            Type = type;
            var u = settings.Meteo.WindDirectionsSettings.GetCount();
            var s = settings.Meteo.WindSpeedsSettings.GetCount();
            CalculationTime = settings.OptimaizedCoeff
                * AppSettingsProvider.OnePointCalcTime
                * settings.PointsCount
                * settings.SourcesCount
                * settings.PollutantsCount
                * u * s / requiredCoresCount;

            RequiredRamCount = 0.063 * settings.SourcesCount + 7;
            RequiredCoresCount = requiredCoresCount;
            CreatedAt = Executor.ExecutionTime;
        }
    }
}
