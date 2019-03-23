namespace EventBus.RabbitMQ
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using EventBus;
    using EventBus.Events;
    using Extensions;
    using Logging;
    using Polly;
    using global::RabbitMQ.Client;
    using global::RabbitMQ.Client.Events;
    using global::RabbitMQ.Client.Exceptions;
    using IModel = global::RabbitMQ.Client.IModel;

    public class EventBusRabbitMQ : IEventBus
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger _logger;
        private readonly IEventLogger _eventLogger;
        private readonly IIntegrationEventHandlerFactory _integrationEventHandlerFactory;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly int _retryCount;
        private readonly string _brokerName;
        private readonly string _queueName;
        private readonly int _prefetchCount;
        private IModel _consumerChannel;

        public EventBusRabbitMQ(
            string brokerName,
            IRabbitMQPersistentConnection persistentConnection,
            IEventBusSubscriptionsManager subsManager,
            IIntegrationEventHandlerFactory integrationEventHandlerFactory,
            IEventLogger eventLogger,
            ILogger logger) : this(
                brokerName,
                string.Empty,
                0,
                persistentConnection,
                subsManager,
                integrationEventHandlerFactory,
                eventLogger,
                logger)
        {
        }

        public EventBusRabbitMQ(
            string brokerName,
            string queueName,
            int prefetchCount,
            IRabbitMQPersistentConnection persistentConnection,
            IEventBusSubscriptionsManager subsManager,
            IIntegrationEventHandlerFactory integrationEventHandlerFactory,
            IEventLogger eventLogger,
            ILogger logger)
        {
            _brokerName = brokerName;
            _queueName = queueName;
            _prefetchCount = prefetchCount;
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventLogger = eventLogger ?? throw new ArgumentNullException(nameof(eventLogger));
            _integrationEventHandlerFactory = integrationEventHandlerFactory;
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _retryCount = 5;
        }

        public void Publish<T>(T @event) where T : IntegrationEvent
        {
            _eventLogger.TracePublish(@event);

            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.Warn(ex);
                });

            using (var channel = _persistentConnection.CreateModel())
            {
                var eventName = @event.GetType().Name;
                var message = @event.SerializeToJson();
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    // ReSharper disable AccessToDisposedClosure
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(
                        exchange: _brokerName,
                        routingKey: eventName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                    // ReSharper restore AccessToDisposedClosure
                });
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _subsManager.AddSubscription<T, TH>();
        }

        public void StartConsumerChannel()
        {
            CreateConsumerChannel();
        }

        public void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent
        {
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void Dispose()
        {
            _consumerChannel?.Dispose();

            _subsManager.Clear();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            if (_prefetchCount > 0)
            {
                channel.BasicQos(0, (ushort)_prefetchCount, false);
            }

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body);

                if (await ProcessEvent(eventName, message))
                {
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
            };

            channel.BasicConsume(queue: _queueName,
                                 autoAck: false,
                                 consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel?.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);

                foreach (var subscription in subscriptions)
                {
                    var handler = _integrationEventHandlerFactory.GetIntegrationEventHandler(subscription.HandlerType.FullName);
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var integrationEvent = message.DeserializeJson(eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    var method = concreteType.GetMethod("Handle");
                    if (method != null)
                    {
                        await ((Task)method.Invoke(handler, new[] { integrationEvent })).ConfigureAwait(false);
                    }
                }

                return true;
            }

            return false;
        }
    }
}