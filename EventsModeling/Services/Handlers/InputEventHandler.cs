using EventsModeling.Models.Events;
using EventsModeling.Models.Transactions;
using EventsModeling.Resources;
using EventsModeling.Services.Transactions;

namespace EventsModeling.Services.Handlers
{
    public class InputEventHandler : IEventHandler
    {
        private readonly TransactionCreator _transactionCreator = new TransactionCreator();

        public void HandleEvent(IEvent @event)
        {
            var transaction = _transactionCreator.CreateTransactionByType();

            AddStatistics(transaction);

            if (ServerResources.IsHandleAllowed(transaction))
            {
                ServerResources.AllocateForTransaction(transaction);
                EventsCollector.AddEvent(new OutputEvent(transaction));
            }
            else
                TransactionsQueue.Enqueue(transaction);

            EventsCollector.AddEvent(new InputEvent());
        }

        private void AddStatistics(Transaction transaction)
            => Executor.ResultsCollector.AddCreatedTransactionResult(transaction);

        public void AddStatistics(IEvent @event)
        {}
    }
}
