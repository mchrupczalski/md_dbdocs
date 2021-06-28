using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDocsGenerator.Models
{
    internal class DatesInfoModel
    {
        [JsonProperty("WeekDay")]
        public string WeekDay { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }
    }
}
