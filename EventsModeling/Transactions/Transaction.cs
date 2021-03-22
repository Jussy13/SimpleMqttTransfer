namespace EventsModeling.Transactions
{
    public class Transaction
    {
        public TransactionType Type { get; }
        public int RequiredCoresCount { get; }
        public int RequiredRamCount { get; }

        public Transaction(TransactionType type, int requiredCoresCount, int requiredRamCount)
        {
            Type = type;
            RequiredCoresCount = requiredCoresCount;
            RequiredRamCount = requiredRamCount;
        }
    }

    public enum TransactionType
    {
        Monitoring,
        Calculation
    }
}
