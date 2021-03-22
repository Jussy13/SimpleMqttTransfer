using EventsModeling.Transactions;

namespace EventsModeling.Resources
{
    public static class ServerResources
    {
        private static int _coresCount;
        private static int _ramCount;

        public static void InitServerResources(int coresCount, int ramCount)
        {
            _coresCount = coresCount;
            _ramCount = ramCount;
        }

        public static bool IsHandleAllowed(Transaction transaction)
            => _coresCount >= transaction.RequiredCoresCount && _ramCount >= transaction.RequiredRamCount;

        public static void AllocateForTransaction(Transaction transaction)
        {
            _coresCount -= transaction.RequiredCoresCount;
            _ramCount -= transaction.RequiredRamCount;
        }

        public static void Dispose(Transaction transaction)
        {
            _coresCount += transaction.RequiredCoresCount;
            _ramCount += transaction.RequiredRamCount;
        }
    }
}
