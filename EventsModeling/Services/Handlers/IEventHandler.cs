using EventsModeling.Events;

namespace EventsModeling.Services.Handlers
{
    public interface IEventHandler
    {
        void HandleEvent(IEvent @event);
    }
}
