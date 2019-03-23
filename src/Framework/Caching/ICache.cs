// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICache.cs" company="LEGO System A/S">
//   Copyright (c) LEGO System A/S. All rights reserved.
// </copyright>
// <summary>
//   Basic cache interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Caching
{
    using System;

    /// <summary>
    /// The Cache interface.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Check if cache contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Contains(string key);

        /// <summary>
        /// Get data from cache by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <typeparam name="T">Data type.</typeparam>
        /// <returns>The <see cref="T"/>.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Add data to cache. never expire.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Add(string key, object value);

        /// <summary>
        /// Add data to cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiration">The item expiration.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Add(string key, object value, TimeSpan expiration);

        /// <summary>
        /// Remove and return data from cache.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove(string key);

        /// <summary>
        /// Flush all keys from cache.
        /// </summary>
        void Flush();
    }
}