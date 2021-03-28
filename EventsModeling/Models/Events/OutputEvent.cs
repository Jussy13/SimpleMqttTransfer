using System;
using EventsModeling.Models.Transactions;
using EventsModeling.Services;

namespace EventsModeling.Models.Events
{
    public class OutputEvent : IEvent
    {
        public DateTime CreatedAt { get; }
        public DateTime FinishedAt { get; }
        public Transaction Transaction { get; }

        public OutputEvent(Transaction transaction)
        {
            Transaction = transaction;
            CreatedAt = Executor.ExecutionTime;
            FinishedAt = CreatedAt + TimeSpan.FromSeconds(transaction.CalculationTime);
        }
    }
}
