namespace EventBus
{
    using System;

    using EventBus.Events;

    public class ConsoleEventLogger : IEventLogger
    {
        public void TracePublish<T>(T integrationEvent)
            where T : IntegrationEvent
        {
            Console.WriteLine($"Published {integrationEvent}");
        }

        public void TraceHandle<T>(T integrationEvent)
            where T : IntegrationEvent
        {
            Console.WriteLine($"Handled {integrationEvent}");
        }
    }
}