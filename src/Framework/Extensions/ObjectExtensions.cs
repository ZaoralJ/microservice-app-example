// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="LEGO System A/S">
//   Copyright (c) LEGO System A/S. All rights reserved.
// </copyright>
// <summary>
//   Extension of object type ;-)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Extensions
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// The object extensions.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Serialize to JSON.
        /// </summary>
        /// <param name="o"> The object.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string SerializeToJson(this object o)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            return JsonConvert.SerializeObject(o, settings);
        }

        /// <summary>
        /// Deserialize JSON.
        /// </summary>
        /// <param name="jsonData">The JSON data.</param>
        /// <typeparam name="T">T.</typeparam>
        /// <returns>The <see cref="T"/>.</returns>
        public static T DeserializeJson<T>(this string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        /// <summary>
        /// Deserialize JSON.
        /// </summary>
        /// <param name="jsonData">The JSON data.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object DeserializeJson(this string jsonData, Type type)
        {
            return JsonConvert.DeserializeObject(jsonData, type);
        }
    }
}