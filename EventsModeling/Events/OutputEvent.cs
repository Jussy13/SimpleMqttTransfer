
using EventsModeling.Transactions;

namespace EventsModeling.Events
{
    public class OutputEvent : EventBase
    {
        public override EventType EventType { get; } = EventType.Output;
        public OutputEvent(Transaction transaction, double handleTime) : base(transaction, handleTime)
        { }
    }
}
