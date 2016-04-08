using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBotNet.Models.Youtube
{
    [JsonObject]
    public class YoutubeSettings
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
    }
}
