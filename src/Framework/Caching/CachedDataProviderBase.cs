// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachedDataProviderBase.cs" company="LEGO System A/S">
//   Copyright (c) LEGO System A/S. All rights reserved.
// </copyright>
// <summary>
//   Base class for cached data providers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Caching
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The cached data provider base.
    /// </summary>
    public abstract class CachedDataProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedDataProviderBase"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        protected CachedDataProviderBase(ICache cache)
        {
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// The cache.
        /// </summary>
        protected ICache Cache { get; }

        /// <summary>
        /// Get cached data.
        /// </summary>
        /// <typeparam name="T">Any object.</typeparam>
        /// <param name="getOriginData">The get origin data.</param>
        /// <param name="key">The key.</param>
        /// <returns>The <see cref="T"/>.</returns>
        protected T GetCachedData<T>(Func<T> getOriginData, string key)
        {
            return GetCachedData(getOriginData, key, TimeSpan.MaxValue);
        }

        /// <summary>
        /// Get cached data. Data never expire in cache.
        /// </summary>
        /// <typeparam name="T">Any object.</typeparam>
        /// <param name="getOriginData">The get origin data.</param>
        /// <param name="key">The key.</param>
        /// <param name="nullValue">Default value of T for null result from <paramref name="getOriginData"/>.</param>
        /// <returns>The <see cref="T"/>. </returns>
        protected T GetCachedData<T>(Func<T> getOriginData, string key, T nullValue)
        {
            return GetCachedData(getOriginData, key, TimeSpan.MaxValue, nullValue);
        }

        /// <summary>
        /// Get cached data.
        /// </summary>
        /// <typeparam name="T">Any object.</typeparam>
        /// <param name="getOriginData">The get origin data.</param>
        /// <param name="key">The key.</param>
        /// <param name="expiration">The expiration of cached data.</param>
        /// <param name="nullValue">Default value of T for null result from <paramref name="getOriginData"/>.</param>
        /// <returns>The <see cref="T"/>. </returns>
        protected T GetCachedData<T>(Func<T> getOriginData, string key, TimeSpan expiration, T nullValue = default(T))
        {
            try
            {
                // get data from cache.
                if (Cache.Contains(key))
                {
                    if (Cache.Contains(key))
                    {
                        var cachedData = Cache.Get<T>(key);
                        return cachedData.Equals(nullValue) ? default(T) : cachedData;
                    }
                }
            }
            catch
            {
                // log it
            }

            // get data from origin source
            var data = getOriginData.Invoke();

            AddDataToCache(key, expiration, nullValue, data);

            return data;
        }

        /// <summary>
        /// Get cached data asynchronously.
        /// </summary>
        /// <typeparam name="T">Any object.</typeparam>
        /// <param name="getOriginData">The get origin data.</param>
        /// <param name="key">The key.</param>
        /// <returns>The <see cref="T"/>.</returns>
        protected async Task<T> GetCachedDataAsync<T>(Func<Task<T>> getOriginData, string key)
        {
            return await GetCachedDataAsync(getOriginData, key, TimeSpan.MaxValue, default(T)).ConfigureAwait(false);
        }

        /// <summary>
        /// Get cached data. Data never expire in cache.
        /// </summary>
        /// <typeparam name="T">Any object.</typeparam>
        /// <param name="getOriginData">The get origin data.</param>
        /// <param name="key">The key.</param>
        /// <param name="expiration">The cache expiration.</param>
        /// <returns>The <see cref="T"/>. </returns>
        protected async Task<T> GetCachedDataAsync<T>(Func<Task<T>> getOriginData, string key, TimeSpan expiration)
        {
            return await GetCachedDataAsync(getOriginData, key, expiration, default(T)).ConfigureAwait(false);
        }

        /// <summary>
        /// Get cached data. Data never expire in cache.
        /// </summary>
        /// <typeparam name="T">Any object.</typeparam>
        /// <param name="getOriginData">The get origin data.</param>
        /// <param name="key">The key.</param>
        /// <param name="nullValue">Default value of T for null result from <paramref name="getOriginData"/>.</param>
        /// <returns>The <see cref="T"/>. </returns>
        protected async Task<T> GetCachedDataAsync<T>(Func<Task<T>> getOriginData, string key, T nullValue)
        {
            return await GetCachedDataAsync(getOriginData, key, TimeSpan.MaxValue, nullValue).ConfigureAwait(false);
        }

        /// <summary>
        /// Get cached data.
        /// </summary>
        /// <typeparam name="T">Any object.</typeparam>
        /// <param name="getOriginData">The get origin data.</param>
        /// <param name="key">The key.</param>
        /// <param name="expiration">The expiration of cached data.</param>
        /// <param name="nullValue">Default value of T for null result from <paramref name="getOriginData"/>.</param>
        /// <returns>The <see cref="T"/>. </returns>
        protected async Task<T> GetCachedDataAsync<T>(Func<Task<T>> getOriginData, string key, TimeSpan expiration, T nullValue)
        {
            try
            {
                if (Cache.Contains(key))
                {
                    var cachedData = Cache.Get<T>(key);
                    return cachedData.Equals(nullValue) ? default(T) : cachedData;
                }
            }
            catch
            {
                // log it
            }

            var data = await getOriginData().ConfigureAwait(false);

            AddDataToCache(key, expiration, nullValue, data);

            return data;
        }

        /// <summary>
        /// Add data to cache helper.
        /// </summary>
        /// <typeparam name="T">Type of data.</typeparam>
        private void AddDataToCache<T>(string key, TimeSpan expiration, T nullValue, T data)
        {
            try
            {
                // add data to cache
                if (data == null && nullValue != null)
                {
                    if (expiration == TimeSpan.MaxValue)
                    {
                        Cache.Add(key, nullValue);
                    }
                    else
                    {
                        Cache.Add(key, nullValue, expiration);
                    }
                }
                else if (data != null)
                {
                    if (expiration == TimeSpan.MaxValue)
                    {
                        Cache.Add(key, data);
                    }
                    else
                    {
                        Cache.Add(key, data, expiration);
                    }
                }
            }
            catch
            {
                // log it
            }
        }
    }
}