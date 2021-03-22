using EventsModeling.Events;
using EventsModeling.Resources;
using EventsModeling.Services.Events;

namespace EventsModeling.Services.Handlers
{
    public class InputEventHandler : IEventHandler
    {
        private readonly TransactionCreator _transactionCreator = new TransactionCreator();

        public void HandleEvent(IEvent @event)
        {
            var transaction = _transactionCreator.CreateTransactionByType();

            if (ServerResources.IsHandleAllowed(transaction))
            {
                EventsScheduler.ScheduleEventByTape(EventType.Output, transaction);
                ServerResources.AllocateForTransaction(transaction);
            }
            else
            {
                EventsQueue.Enqueue(transaction);
            }

            EventsScheduler.ScheduleEventByTape(EventType.Input, null);
        }
    }
}
