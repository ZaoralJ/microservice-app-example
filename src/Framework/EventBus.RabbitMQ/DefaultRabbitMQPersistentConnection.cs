namespace EventBus.RabbitMQ
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using Logging;
    using Polly;
    using global::RabbitMQ.Client;
    using global::RabbitMQ.Client.Events;
    using global::RabbitMQ.Client.Exceptions;

    public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly object _syncRoot = new object();
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger _logger;
        private readonly int _retryCount;
        private IConnection _connection;
        private bool _disposed;

        public DefaultRabbitMQPersistentConnection(
            string hostName,
            string userName,
            string password,
            ILogger logger,
            int retryCount = 5) : this(hostName, null, userName, password, logger, retryCount)
        {
        }

        public DefaultRabbitMQPersistentConnection(
            string hostName,
            string virtualHost,
            string userName,
            string password,
            ILogger logger,
            int retryCount = 5)
        {
            if (string.IsNullOrEmpty(virtualHost))
            {
                _connectionFactory = new ConnectionFactory
                {
                    HostName = hostName,
                    UserName = userName,
                    Password = password
                };
            }
            else
            {
                _connectionFactory = new ConnectionFactory
                {
                    HostName = hostName,
                    VirtualHost = virtualHost,
                    UserName = userName,
                    Password = password
                };
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;

            TryConnect();
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool TryConnect()
        {
            _logger.Info("RabbitMQ Client is trying to connect");

            lock (_syncRoot)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.Warn(ex.ToString());
                    });

                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    _logger.Info($"RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events");

                    return true;
                }

                _logger.Fatal("FATAL ERROR: RabbitMQ connections could not be created and opened");

                return false;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                try
                {
                    _connection.Dispose();
                }
                catch (IOException ex)
                {
                    _logger.Fatal(ex.ToString());
                }

                _disposed = true;
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            _logger.Warn("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            _logger.Warn("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed)
            {
                return;
            }

            _logger.Warn("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }
    }
}