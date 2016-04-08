using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBotNet.Models.ChatBot
{
    [JsonObject]
    public class ChatBotSettings
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("chat_bot_id")]
        public string ChatBotId { get; set; }
    }
}
