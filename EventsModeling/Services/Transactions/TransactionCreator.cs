using System;
using EventsModeling.Models.Transactions;

namespace EventsModeling.Services.Transactions
{
    public class TransactionCreator
    {
        private readonly Random _randGen = new Random();

        public Transaction CreateTransactionByType()
        {
            var rnd = _randGen.NextDouble();
            var buf = 0.0;
            var type = 0;

            foreach (var item in TransactionHelper.FreqByType)
            {
                if (buf <= rnd && rnd < buf + item.Value)
                    return new Transaction(item.Key,
                        TransactionHelper.SettingsByType[item.Key],
                        TransactionHelper.CurRequiredCoresSet[type]);

                buf += item.Value;
                type++;
            }

            throw new ApplicationException("Bad transaction");
        }
    }
}
