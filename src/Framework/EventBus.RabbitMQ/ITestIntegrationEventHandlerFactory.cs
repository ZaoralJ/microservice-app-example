namespace EventBus.RabbitMQ
{
    public interface IIntegrationEventHandlerFactory
    {
        IIntegrationEventHandler GetIntegrationEventHandler(string name);
    }
}