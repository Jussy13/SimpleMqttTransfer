namespace EventsModeling.Models.Transactions
{
    public class TransactionSettings
    {
        public Meteo Meteo { get; set; } = new Meteo();
        public int PointsCount { get; set; }
        public int SourcesCount { get; set; }
        public int PollutantsCount { get; set; }
        public int FactoriesCount { get; set; }

        public double OptimaizedCoeff { get; set; } = 1.0;
    }
}
