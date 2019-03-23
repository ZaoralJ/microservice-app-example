// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedisCache.cs" company="LEGO System A/S">
//   Copyright (c) LEGO System A/S. All rights reserved.
// </copyright>
// <summary>
//   Implementation of ICache against REDIS.
//   If not possible to connect to endpoint cache will return always false in Contains()
//   and throw exception when trying to add or get data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Caching.Redis
{
    using System;
    using Extensions;
    using StackExchange.Redis;

    /// <summary>
    /// The REDIS cache.
    /// </summary>
    public class RedisCache : ICache, IDisposable
    {
        /// <summary>
        /// The REDIS endpoint.
        /// </summary>
        private readonly string _endpoint;

        /// <summary>
        /// The redis connection.
        /// </summary>
        private ConnectionMultiplexer _redis;

        /// <summary>
        /// The REDIS database.
        /// </summary>
        private IDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCache"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint including port, f.e. localhost:1234 .</param>
        public RedisCache(string endpoint)
        {
            _endpoint = endpoint;

            Connect();
        }

        /// <inheritdoc />
        public bool Contains(string key)
        {
            CheckIfConnected();

            return _database.KeyExists(key);
        }

        /// <inheritdoc />
        public T Get<T>(string key)
        {
            CheckIfConnected();

            if (Contains(key))
            {
                var json = _database.StringGet(key).ToString();
                var res = json.DeserializeJson<T>();
                return res;
            }

            return default(T);
        }

        /// <inheritdoc />
        public bool Add(string key, object value)
        {
            CheckIfConnected();

            var res = _database.StringSet(key, value.SerializeToJson());
            return res;
        }

        /// <inheritdoc />
        public bool Add(string key, object value, TimeSpan expiration)
        {
            CheckIfConnected();

            var res = _database.StringSet(key, value.SerializeToJson());
            _database.KeyExpire(key, expiration);
            return res;
        }

        /// <inheritdoc />
        public void Remove(string key)
        {
            CheckIfConnected();

            _database.KeyDelete(key);
        }

        /// <inheritdoc />
        public void Flush()
        {
            var options = ConfigurationOptions.Parse(_endpoint);
            options.AllowAdmin = true;
            options.ResponseTimeout = 10;
            var redis = ConnectionMultiplexer.Connect(options);
            var db = redis.GetServer(_endpoint);
            db.FlushDatabase();
        }

        /// <inheritdoc />
        /// <summary>
        /// Dispose connection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _redis?.Dispose();
            }
        }

        #region private helpers

        /// <summary>
        /// Connect to REDIS.
        /// </summary>
        private void Connect()
        {
            var options = ConfigurationOptions.Parse(_endpoint);
            options.ResponseTimeout = 5000; // ms
            _redis = ConnectionMultiplexer.Connect(options);
            _database = _redis.GetDatabase();
        }

        /// <summary>
        /// Reconnect if there is no connection.
        /// </summary>
        private void CheckIfConnected()
        {
            if (!_redis.IsConnected)
            {
                Connect();
            }
        }

        #endregion
    }
}