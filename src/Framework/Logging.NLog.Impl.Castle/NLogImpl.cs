namespace Logging.NLog.Impl.Castle
{
    using System;
    using System.Globalization;

    using global::NLog;

    /// <summary>
    /// The NLog implementation.
    /// </summary>
    public class NLogImpl : Logging.ILogger
    {
        /// <summary>
        /// The NLog logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogImpl"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public NLogImpl(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        /// <inheritdoc />
        public bool IsInfoEnabled => _logger.IsInfoEnabled;

        /// <inheritdoc />
        public bool IsWarnEnabled => _logger.IsWarnEnabled;

        /// <inheritdoc />
        public bool IsErrorEnabled => _logger.IsErrorEnabled;

        /// <inheritdoc />
        public bool IsFatalEnabled => _logger.IsFatalEnabled;

        /// <inheritdoc />
        public void Debug(object message)
        {
            _logger.Debug(CultureInfo.InvariantCulture, message);
        }

        /// <inheritdoc />
        public void Debug(object message, Exception exception)
        {
            _logger.Debug(exception, message.ToString);
        }

        /// <inheritdoc />
        public void Info(object message)
        {
            _logger.Info(CultureInfo.InvariantCulture, message);
        }

        /// <inheritdoc />
        public void Info(object message, Exception exception)
        {
            _logger.Info(exception, message.ToString);
        }

        /// <inheritdoc />
        public void Warn(object message)
        {
            _logger.Warn(CultureInfo.InvariantCulture, message);
        }

        /// <inheritdoc />
        public void Warn(object message, Exception exception)
        {
            _logger.Warn(exception, message.ToString());
        }

        /// <inheritdoc />
        public void Error(object message)
        {
            _logger.Error(CultureInfo.InvariantCulture, message);
        }

        /// <inheritdoc />
        public void Error(object message, Exception exception)
        {
            _logger.Error(exception, message.ToString());
        }

        /// <inheritdoc />
        public void Fatal(object message)
        {
            _logger.Fatal(CultureInfo.InvariantCulture, message);
        }

        /// <inheritdoc />
        public void Fatal(object message, Exception exception)
        {
            _logger.Fatal(exception, message.ToString());
        }
    }
}