namespace EventBus
{
    using EventBus.Events;

    public interface IEventBus
    {
        void Publish<T>(T integrationEvent) 
            where T : IntegrationEvent;

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void StartConsumerChannel();
    }
}