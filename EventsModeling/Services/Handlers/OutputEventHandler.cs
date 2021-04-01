using System.Collections.Generic;
using EventsModeling.Models.Events;
using EventsModeling.Models.Transactions;
using EventsModeling.Resources;
using EventsModeling.Services.Transactions;

namespace EventsModeling.Services.Handlers
{
    public class OutputEventHandler : IEventHandler
    {
        public void HandleEvent(IEvent @event)
        {
            if (null == @event)
                return;

            if (null != @event.Transaction)
            {
                ServerResources.Dispose(@event.Transaction);
                AddStatistics(@event);
            }

            var tempQueue = new Queue<Transaction>();

            while (TransactionsQueue.TryDequeue(out var transaction))
            {
                if (ServerResources.IsHandleAllowed(transaction))
                {
                    ServerResources.AllocateForTransaction(transaction);
                    EventsCollector.AddEvent(new OutputEvent(transaction));
                }
                else
                    tempQueue.Enqueue(transaction);
            }

            while (tempQueue.TryDequeue(out var transaction))
                TransactionsQueue.Enqueue(transaction);
        }

        public void AddStatistics(IEvent @event)
            =>
                Executor.ResultsCollector.AddHandledTransactionResult(@event.Transaction,
                    @event.FinishedAt - @event.Transaction.CreatedAt);
    }
}
