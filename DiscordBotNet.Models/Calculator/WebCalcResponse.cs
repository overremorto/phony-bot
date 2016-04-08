using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBotNet.Models.Calculator
{
    public class WebCalcResponse
    {
        public IEnumerable<Web2Result> Results { get; set; }
        public string GetMessageText()
        {
            return Results?.FirstOrDefault()?.Out ?? "UNKOWN";
        }
    }

    [JsonObject]
    public class Web2Result
    {
        [JsonProperty("img64")]
        public string Image64 { get; set; }
        public string In { get; set; }

        public string Out { get; set; }

        public string Status { get; set; }
    }
}
