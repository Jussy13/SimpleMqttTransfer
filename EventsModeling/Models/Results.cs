namespace EventsModeling.Models
{
    public class Results
    {
        public int CoresCount { get; set; }
        public double RamCount { get; set; }
        public int CreatedTransactionsCount { get; set; }
        public int HandledTransactionCount { get; set; }
        public double AvgTransactionCalcTime { get; set; }
        public double SpendTransactionCalcTime { get; set; }
        public double CalculationTime { get; set; }
    }
}
