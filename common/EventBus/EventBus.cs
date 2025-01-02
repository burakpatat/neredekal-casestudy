
namespace EventBus
{
    public interface IEventBus
    {
        void Publish<T>(T @event);
        void Subscribe<T>(Func<T, Task> handler);
    }

    public class EventBus : IEventBus
    {
        private readonly EventBus _eventBus;

        public EventBus(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Publish<T>(T @event)
        {
            _eventBus.Publish(@event);
        }

        public void Subscribe<T>(Func<T, Task> handler)
        {
            _eventBus.Subscribe(handler);
        }
    }

}
