using System;
using EventsModeling.Models.Events;

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

            switch (@event)
            {
                case InputEvent _:
                    _inputEventHandler.HandleEvent(@event);
                    break;
                case OutputEvent _:
                    _outputEventHandler.HandleEvent(@event);
                    break;
                default:
                    throw new ApplicationException("Incorrect event type");
            }
        }

        public void AddStatistics(IEvent @event)
        { }
    }
}
