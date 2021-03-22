using System.Collections.Generic;
using EventsModeling.Transactions;

namespace EventsModeling.Services.Events
{
    public static class EventsQueue
    {
        private static readonly Queue<Transaction> _queue = new Queue<Transaction>();

        public static void Enqueue(Transaction transaction) => _queue.Enqueue(transaction);

        public static bool TryDequeue(out Transaction transaction) => _queue.TryDequeue(out transaction);
    }
}
