using System;
using YamlDotNet.Serialization;

namespace md_dbdocs.lib.Serializers
{
    public static class YamlSerializer
    {
        /// <summary>
        /// Serialize C# Class to YAML
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="model">Class object to serialize</param>
        /// <returns>YAML string</returns>
        public static string Serialize<T>(T model)
        {
            var serializer = new SerializerBuilder().Build();
            return serializer.Serialize(model);
        }

        /// <summary>
        /// Deserialize YAML to C# class object
        /// </summary>
        /// <typeparam name="T">Target class</typeparam>
        /// <param name="yaml">YAML string</param>
        /// <returns></returns>
        public static T DeSerialize<T>(string yaml)
        {
            var deserializer = new DeserializerBuilder().Build();
            return deserializer.Deserialize<T>(yaml);
        }
    }
}
