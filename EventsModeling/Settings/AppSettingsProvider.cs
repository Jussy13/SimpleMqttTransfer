namespace EventsModeling.Settings
{
    public class AppSettingsProvider
    {
        public static int CoresCount { get; set; }
        public static int RamCount { get; set; }
        public static double ModelTime { get; set; }
        public static double OnePointCalcTime { get; set; }
        public static double TransactionDelayMean { get; set; }
        public static double TransactionDelaySigma { get; set; }
    }
}
