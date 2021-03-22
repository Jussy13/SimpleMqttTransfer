using System;
using EventsModeling.Transactions;

namespace EventsModeling.Services
{
    public class TransactionCreator
    {
        private readonly Random _randGen = new Random();

        public Transaction CreateTransactionByType()
            =>
                _randGen.NextDouble() < 0.65
                    ? new Transaction(TransactionType.Monitoring, 8, 1)
                    : new Transaction(TransactionType.Calculation, 8, 10);
    }
}
