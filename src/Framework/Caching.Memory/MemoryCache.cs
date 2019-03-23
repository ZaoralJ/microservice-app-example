// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryCache.cs" company="LEGO System A/S">
//   Copyright (c) LEGO System A/S. All rights reserved.
// </copyright>
// <summary>
//   Memory cache implementation. Using System.Runtime.Caching.MemoryCache
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Caching.Memory
{
    using System;

    /// <summary>
    /// Memory cache implementation. Using System.Runtime.Caching.MemoryCache.
    /// </summary>
    public class MemoryCache : ICache, IDisposable
    {
        /// <summary>
        /// The memory cache.
        /// </summary>
        private readonly System.Runtime.Caching.MemoryCache _memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCache"/> class.
        /// </summary>
        public MemoryCache()
        {
            _memoryCache = new System.Runtime.Caching.MemoryCache("MemoryCache");
        }

        /// <inheritdoc />
        public bool Contains(string key)
        {
            return _memoryCache.Contains(key);
        }

        /// <inheritdoc />
        public T Get<T>(string key)
        {
            return (T)_memoryCache.Get(key);
        }

        /// <inheritdoc />
        public bool Add(string key, object value)
        {
            return _memoryCache.Add(key, value, DateTimeOffset.MaxValue);
        }

        /// <inheritdoc />
        public bool Add(string key, object value, TimeSpan expiration)
        {
            return _memoryCache.Add(key, value, DateTimeOffset.Now + expiration);
        }

        /// <inheritdoc />
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        /// <inheritdoc />
        public void Flush()
        {
            _memoryCache.Trim(100);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _memoryCache?.Dispose();
            }
        }
    }
}