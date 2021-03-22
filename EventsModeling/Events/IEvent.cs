using System;
using EventsModeling.Transactions;

namespace EventsModeling.Events
{
    public interface IEvent
    {
        DateTime CreatedAt { get; }
        DateTime FinishedAt { get; }

        EventType EventType { get; }

        Transaction Transaction { get; }
    }
}
