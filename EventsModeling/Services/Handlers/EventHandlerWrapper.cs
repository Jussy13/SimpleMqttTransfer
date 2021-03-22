using System;
using EventsModeling.Events;

namespace EventsModeling.Services.Handlers
{
    public class EventHandlerWrapper : IEventHandler
    {
        private readonly IEventHandler _inputEventHandler = new InputEventHandler();
        private readonly IEventHandler _outputEventHandler = new OutputEventHandler();

        public void HandleEvent(IEvent @event)
        {
            // This only for start modeling
            if (null == @event)
            {
                _inputEventHandler.HandleEvent(null);
                return;
            }

            Executor.ExecutionTime = @event.FinishedAt;
            Console.WriteLine($"Event {@event.EventType} (created at {@event.CreatedAt}) (handled at {@event.FinishedAt})"
                              + $" calc {@event.FinishedAt - @event.CreatedAt}");

            switch (@event.EventType)
            {
                case EventType.Input:
                    _inputEventHandler.HandleEvent(@event);
                    break;
                case EventType.Output:
                    _outputEventHandler.HandleEvent(@event);
                    break;
                default:
                    throw new ApplicationException("Incorrect event type");
            }
        }
    }
}
