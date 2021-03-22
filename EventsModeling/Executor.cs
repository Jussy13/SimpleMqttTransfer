using System;
using EventsModeling.Services.Events;
using EventsModeling.Services.Handlers;

namespace EventsModeling
{
    public class Executor
    {
        public static DateTime ExecutionTime;
        private readonly IEventHandler _eventHandler;

        static Executor() => ExecutionTime = DateTime.Now;
        public Executor(IEventHandler eventHandler) => _eventHandler = eventHandler;

        public void Execute(DateTime timeOut)
        {
            while (ExecutionTime < timeOut)
            {
                _eventHandler.HandleEvent(EventsCollector.GetEvent());
            }
        }
    }
}
