using System;

namespace EventsModeling.Models
{
    public class Results
    {
        public int CoresCount { get; set; }
        public int RamCount { get; set; }
        public int CreatedTransactionsCount { get; set; }
        public int HandledTransactionCount { get; set; }
        public TimeSpan AvgTransactionCalcTime { get; set; }
        public TimeSpan SpendTransactionCalcTime { get; set; }
        public double CalculationTime { get; set; }
    }
}
