using System;
using System.Collections.Generic;
using System.Linq;
using EventsModeling.Models;
using EventsModeling.Models.Transactions;

namespace EventsModeling.Services
{
    public class ResultsCollector
    {
        private readonly Dictionary<string, Results> _transactionsResultsByType = new Dictionary<string, Results>();

        public void AddCreatedTransactionResult(Transaction transaction)
        {
            if (!_transactionsResultsByType.ContainsKey(transaction.Type))
            {
                _transactionsResultsByType.Add(transaction.Type, new Results());
                _transactionsResultsByType[transaction.Type].CoresCount = transaction.RequiredCoresCount;
                _transactionsResultsByType[transaction.Type].RamCount = transaction.RequiredRamCount;
                _transactionsResultsByType[transaction.Type].CalculationTime = transaction.CalculationTime;
            }

            _transactionsResultsByType[transaction.Type].CreatedTransactionsCount += 1;
        }

        public void AddHandledTransactionResult(Transaction transaction, TimeSpan time)
        {
            if (!_transactionsResultsByType.ContainsKey(transaction.Type))
            {
                _transactionsResultsByType.Add(transaction.Type, new Results());
                _transactionsResultsByType[transaction.Type].CoresCount = transaction.RequiredCoresCount;
                _transactionsResultsByType[transaction.Type].RamCount = transaction.RequiredRamCount;
                _transactionsResultsByType[transaction.Type].CalculationTime = transaction.CalculationTime;
            }

            _transactionsResultsByType[transaction.Type].HandledTransactionCount += 1;
            _transactionsResultsByType[transaction.Type].SpendTransactionCalcTime += time.TotalSeconds;
        }

        public IOrderedEnumerable<KeyValuePair<string, Results>> GetResults()
        {
            foreach (var item in _transactionsResultsByType)
                item.Value.AvgTransactionCalcTime =
                    item.Value.SpendTransactionCalcTime / item.Value.HandledTransactionCount;

            return _transactionsResultsByType.OrderBy(i => i.Key);
        }

        public void Clear() => _transactionsResultsByType.Clear();
    }
}
