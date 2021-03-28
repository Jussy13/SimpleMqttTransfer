using System;
using System.Collections.Generic;
using EventsModeling.Resources;
using EventsModeling.Services.Events;
using EventsModeling.Services.Handlers;
using EventsModeling.Services.Transactions;
using EventsModeling.Settings;

namespace EventsModeling.Services
{
    public class Executor
    {
        public static DateTime ExecutionTime;
        public static readonly ResultsCollector ResultsCollector;
        private readonly IEventHandler _eventHandler;

        static Executor() => ResultsCollector = new ResultsCollector();
        public Executor(IEventHandler eventHandler) => _eventHandler = eventHandler;

        public void Execute()
        {
            foreach (var set in TransactionHelper.RequiredCoresSets)
            {
                ServerResources.InitServerResources(AppSettingsProvider.CoresCount, AppSettingsProvider.RamCount);
                ExecutionTime = DateTime.Now;
                var endOfExecution = ExecutionTime + TimeSpan.FromMinutes(AppSettingsProvider.ModelTime);
                TransactionHelper.CurRequiredCoresSet = set;

                Console.WriteLine($"Sterted at {ExecutionTime}");
                try
                {
                    while (ExecutionTime < endOfExecution)
                        _eventHandler.HandleEvent(EventsCollector.GetEvent());
                    PrintStatistics(set);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Console.WriteLine($"Finished at {endOfExecution}");

                ResultsCollector.Clear();
                EventsCollector.Clear();
                TransactionsQueue.Clear();
            }
        }

        private static void PrintStatistics(IEnumerable<int> set)
        {
            Console.WriteLine($"Cores " + string.Join('-', set));
            foreach (var item in ResultsCollector.GetCreatedTransactionsResults())
                Console.WriteLine($"Created transactions {item.Key} count {item.Value}");
            foreach (var item in ResultsCollector.GetHandledTransactionsResults())
                Console.WriteLine($"Handled transactions {item.Key} count {item.Value}");
            foreach (var item in ResultsCollector.GetAvgTransactionsCalcTimeByTypeResult())
                Console.WriteLine($"Avg transactions calc time {item.Key} result {item.Value}");
        }
    }
}
