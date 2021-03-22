using System.Collections.Generic;
using EventsModeling.Events;
using EventsModeling.Resources;
using EventsModeling.Services.Events;
using EventsModeling.Transactions;

namespace EventsModeling.Services.Handlers
{
    public class OutputEventHandler : IEventHandler
    {
        public void HandleEvent(IEvent @event)
        {
            if (null == @event)
                return;

            if (null != @event.Transaction)
                ServerResources.Dispose(@event.Transaction);

            var tempQueue = new Queue<Transaction>();

            while (EventsQueue.TryDequeue(out var transaction))
            {
                if (ServerResources.IsHandleAllowed(transaction))
                {
                    EventsScheduler.ScheduleEventByTape(EventType.Output, transaction);
                }
                else
                {
                    tempQueue.Enqueue(transaction);
                }
            }

            while (tempQueue.TryDequeue(out var transaction))
            {
                EventsQueue.Enqueue(transaction);
            }
        }
    }
}
