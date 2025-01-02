
namespace EventBus
{
    public class EventPublisher
    {
        private readonly IEventBus _eventBus;

        public EventPublisher(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void PublishEvent<T>(T @event)
        {
            _eventBus.Publish(@event);
        }
    }
}
