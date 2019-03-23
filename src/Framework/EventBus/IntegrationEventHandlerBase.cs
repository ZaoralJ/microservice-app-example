namespace EventBus
{
    using System.Threading.Tasks;
    using EventBus.Events;

    public abstract class IntegrationEventHandlerBase<TIntegrationEvent> : IIntegrationEventHandler<TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        protected IntegrationEventHandlerBase(IEventLogger eventLogger)
        {
            EventLogger = eventLogger;
        }

        protected IEventLogger EventLogger { get; }

        public Task Handle(TIntegrationEvent integrationEvent)
        {
            EventLogger.TraceHandle(integrationEvent);
            return HandleBody(integrationEvent);
        }

        protected abstract Task HandleBody(TIntegrationEvent integrationEvent);
    }
}