using System.Collections.Generic;
using EventsModeling.Models.Transactions;

namespace EventsModeling.Services.Transactions
{
    public static class TransactionsQueue
    {
        private static readonly Queue<Transaction> _queue = new Queue<Transaction>();
        public static void Enqueue(Transaction transaction) => _queue.Enqueue(transaction);
        public static bool TryDequeue(out Transaction transaction) => _queue.TryDequeue(out transaction);
        public static void Clear() => _queue.Clear();
    }
}
