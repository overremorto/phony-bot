using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBotNet.Models.Dota
{
    [JsonObject]
    public class DotaSettings
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("my_steam_account_id")]
        public string MySteamAccountId { get; set; }
    }
}
