namespace EventBus
{
    using EventBus.Events;

    public interface IEventLogger
    {
        void TracePublish<T>(T integrationEvent)
            where T : IntegrationEvent;

        void TraceHandle<T>(T integrationEvent)
            where T : IntegrationEvent;
    }
}