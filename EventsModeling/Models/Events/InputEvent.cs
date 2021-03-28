using System;
using EventsModeling.Models.Transactions;
using EventsModeling.Services;
using EventsModeling.Settings;

namespace EventsModeling.Models.Events
{
    public class InputEvent : IEvent
    {
        private static readonly RandomGenerator _inputRandGen = new RandomGenerator();
        public DateTime CreatedAt { get; }
        public DateTime FinishedAt { get; }
        public Transaction Transaction { get; }

        public InputEvent()
        {
            CreatedAt = Executor.ExecutionTime;
            FinishedAt = CreatedAt
                         + TimeSpan.FromSeconds(_inputRandGen.Generate(AppSettingsProvider.TransactionDelayMean,
                             AppSettingsProvider.TransactionDelaySigma));
        }
    }
}
