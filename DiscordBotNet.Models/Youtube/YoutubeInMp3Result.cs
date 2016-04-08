using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBotNet.Models.Youtube
{
    [JsonObject]
    public class YoutubeInMp3Result
    {
        public string Title { get; set; }

        public int Length { get; set; }

        public string Link { get; set; }
    }
}
