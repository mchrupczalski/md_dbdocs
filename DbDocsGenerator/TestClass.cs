// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using CodeBeautify;
//
//    var welcome6 = Welcome6.FromJson(jsonString);

namespace CodeBeautify
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Welcome6
    {
        [JsonProperty("Module")]
        public string Module { get; set; }

        [JsonProperty("Schema_Name")]
        public string SchemaName { get; set; }

        [JsonProperty("Object_Name")]
        public string ObjectName { get; set; }

        [JsonProperty("File_Name")]
        public string FileName { get; set; }

        [JsonProperty("Created_Date")]
        public string CreatedDate { get; set; }

        [JsonProperty("Author")]
        public Author Author { get; set; }
    }

    public partial class Author
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Initials")]
        public string Initials { get; set; }

        [JsonProperty("Email_1")]
        public string Email1 { get; set; }

        [JsonProperty("Email_2")]
        public string Email2 { get; set; }
    }

    public partial class Welcome6
    {
        public static Welcome6 FromJson(string json) => JsonConvert.DeserializeObject<Welcome6>(json, CodeBeautify.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome6 self) => JsonConvert.SerializeObject(self, CodeBeautify.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
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
