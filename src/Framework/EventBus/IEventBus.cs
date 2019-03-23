namespace EventBus
{
    using EventBus.Events;

    public interface IEventBus
    {
        void Publish(IntegrationEvent integrationEvent);

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void StartConsumerChannel();
    }
}