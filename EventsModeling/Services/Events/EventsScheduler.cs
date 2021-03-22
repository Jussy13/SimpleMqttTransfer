using System;
using EventsModeling.Events;
using EventsModeling.Settings;
using EventsModeling.Transactions;

namespace EventsModeling.Services.Events
{
    public static class EventsScheduler
    {
        private static readonly RandomGenerator _inputRandGen = new RandomGenerator();
        private static readonly RandomGenerator _outputRandGen = new RandomGenerator();

        public static void ScheduleEventByTape(EventType type, Transaction transaction)
        {
            IEvent @event;
            switch (type)
            {
                case EventType.Input:
                    @event = new InputEvent(transaction, GenerateInputHandleTime());
                    break;
                case EventType.Output:
                    @event = new OutputEvent(transaction, GenerateOutputHandleTime());
                    break;
                default:
                    throw new ApplicationException("Incorrect event type");
            }

            EventsCollector.AddEvent(@event);
        }

        private static double GenerateInputHandleTime()
            =>
                10.0;
                // _inputRandGen.Generate(
                //     AppSettingsProvider.TransactionDelayMean,
                //     AppSettingsProvider.TransactionDelayStdDev);

        private static double GenerateOutputHandleTime()
            =>
                _outputRandGen.Generate(
                    AppSettingsProvider.TransactionHandleMean,
                    AppSettingsProvider.TransactionHandleSigma);
    }
}
