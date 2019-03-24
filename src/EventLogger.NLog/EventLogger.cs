namespace EventLogger.NLog
{
    using EventBus;
    using EventBus.Events;
    using global::NLog;

    public class EventLogger : IEventLogger
    {
        private readonly Logger _logger;

        public EventLogger()
        {
            _logger = LogManager.GetLogger(nameof(EventLogger));
        }

        public void TracePublish<T>(T integrationEvent)
            where T : IntegrationEvent
        {
            _logger.Info("{value1} {value2} {value3}, {value4} {value5}", "Published", typeof(T).FullName, integrationEvent.ParentId, integrationEvent.Id, integrationEvent.CreationDate);
        }

        public void TraceHandle<T>(T integrationEvent)
            where T : IntegrationEvent
        {
            _logger.Info("{value1} {value2} {value3}, {value4} {value5}", "Handled", typeof(T).FullName, integrationEvent.ParentId, integrationEvent.Id, integrationEvent.CreationDate);
        }
    }
}
