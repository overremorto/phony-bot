using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBotNet.Models.Voice
{
    [JsonObject]
    public class VoiceSettings
    {
        [JsonProperty("available_voices")]
        public ReadOnlyCollection<string> AvailableVoices { get; set; } 

        [JsonProperty("current_voice")]
        public string CurrentVoice { get; set; }
    }
}
