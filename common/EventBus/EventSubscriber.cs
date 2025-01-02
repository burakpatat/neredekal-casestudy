
namespace EventBus
{
    public class EventSubscriber
    {
        private readonly IEventBus _eventBus;

        public EventSubscriber(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void SubscribeToEvent<T>(Func<T, Task> handler)
        {
            _eventBus.Subscribe(handler);
        }
    }
}
