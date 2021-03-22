using System;
using EventsModeling.Transactions;

namespace EventsModeling.Events
{
    public abstract class EventBase : IEvent
    {
        public abstract EventType EventType { get; }
        public DateTime CreatedAt { get; }
        public DateTime FinishedAt { get; }
        public Transaction Transaction { get; }

        protected EventBase(Transaction transaction, double handleTime)
        {
            Transaction = transaction;
            CreatedAt = Executor.ExecutionTime;
            FinishedAt = CreatedAt + TimeSpan.FromSeconds(handleTime);
        }
    }
}
