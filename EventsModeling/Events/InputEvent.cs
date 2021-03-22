
using EventsModeling.Transactions;

namespace EventsModeling.Events
{
    public class InputEvent : EventBase
    {
        public override EventType EventType { get; } = EventType.Input;
        public InputEvent(Transaction transaction, double handleTime) : base(transaction, handleTime)
        { }
    }
}
