namespace EventBus
{
    using EventBus.Events;

    public interface IEventLogger
    {
        void Trace(IntegrationEvent integrationEvent);
    }
}