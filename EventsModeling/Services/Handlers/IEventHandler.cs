using EventsModeling.Models.Events;

namespace EventsModeling.Services.Handlers
{
    public interface IEventHandler
    {
        void HandleEvent(IEvent @event);
        void AddStatistics(IEvent @event);
    }
}
