using System;
using System.Collections.Generic;

namespace EventsModeling.Services
{
    public class ResultsCollector
    {
        private readonly Dictionary<string, int> _createdTransactionCountByType;
        private readonly Dictionary<string, int> _handledTransactionCountByType;
        private readonly Dictionary<string, TimeSpan> _avgTransactionCalcTimeByType;
        private readonly Dictionary<string, TimeSpan> _spendTransactionCalcTimeByType;

        public ResultsCollector()
        {
            _createdTransactionCountByType = new Dictionary<string, int>();
            _handledTransactionCountByType = new Dictionary<string, int>();
            _avgTransactionCalcTimeByType = new Dictionary<string, TimeSpan>();
            _spendTransactionCalcTimeByType = new Dictionary<string, TimeSpan>();
        }

        public void AddCreatedTransactionResult(string transactionName)
        {
            if (!_createdTransactionCountByType.ContainsKey(transactionName))
            {
                _createdTransactionCountByType.Add(transactionName, 1);
                return;
            }

            _createdTransactionCountByType[transactionName] = _createdTransactionCountByType[transactionName] + 1;
        }

        public void AddHandledTransactionResult(string transactionName)
        {
            if (!_handledTransactionCountByType.ContainsKey(transactionName))
            {
                _handledTransactionCountByType.Add(transactionName, 1);
                return;
            }

            _handledTransactionCountByType[transactionName] = _handledTransactionCountByType[transactionName] + 1;
        }

        public void AddHandledTransactionResult(string transactionName, TimeSpan time)
        {
            if (!_spendTransactionCalcTimeByType.ContainsKey(transactionName))
            {
                _spendTransactionCalcTimeByType.Add(transactionName, time);
                return;
            }

            _spendTransactionCalcTimeByType[transactionName] = _spendTransactionCalcTimeByType[transactionName] + time;
        }

        public Dictionary<string, int> GetCreatedTransactionsResults() => _createdTransactionCountByType;
        public Dictionary<string, int> GetHandledTransactionsResults() => _handledTransactionCountByType;
        public Dictionary<string, TimeSpan> GetAvgTransactionsCalcTimeByTypeResult()
        {
            foreach (var item in _spendTransactionCalcTimeByType)
                _avgTransactionCalcTimeByType.Add(item.Key, item.Value / _handledTransactionCountByType[item.Key]);

            return _avgTransactionCalcTimeByType;
        }

        public void Clear()
        {
            _createdTransactionCountByType.Clear();
            _handledTransactionCountByType.Clear();
            _avgTransactionCalcTimeByType.Clear();
            _spendTransactionCalcTimeByType.Clear();
        }
    }
}
