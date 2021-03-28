using System;
using EventsModeling.Models.Transactions;

namespace EventsModeling.Models.Events
{
    public interface IEvent
    {
        DateTime CreatedAt { get; }
        DateTime FinishedAt { get; }
        Transaction Transaction { get; }
    }
}
