using System;
using EventsModeling.Settings;

namespace EventsModeling.Models.Transactions
{
    public class Transaction
    {
        public string Type { get; }
        public int RequiredCoresCount { get; }
        public int RequiredRamCount { get; }
        public double CalculationTime { get; }

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

            var ram = Convert.ToInt32((0.063 * settings.SourcesCount + 7) / 1024);
            RequiredRamCount = ram != 0 ? ram : 1;
            RequiredCoresCount = requiredCoresCount;
        }
    }
}
