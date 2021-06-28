using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace md_dbdocs.app.Helpers
{
    internal class JsonSerializer : IDisposable
    {
        /// <summary>
        /// Deserialize JSON to C# class object
        /// </summary>
        /// <typeparam name="T">Target class</typeparam>
        /// <param name="jsonString">JSON string</param>
        /// <returns></returns>
        internal T FromJson<T>(string jsonString) => JsonConvert.DeserializeObject<T>(jsonString, Settings);

        /// <summary>
        /// Serialize C# class to JSON
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="model">Class object to serialize</param>
        /// <returns>JSON string</returns>
        internal string ToJson<T>(T model) => JsonConvert.SerializeObject(model, Settings);

        public void Dispose() { }

        /// <summary>
        /// Serializers settings
        /// </summary>
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}