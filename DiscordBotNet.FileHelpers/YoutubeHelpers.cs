using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.Youtube;
using Newtonsoft.Json;

namespace DiscordBotNet.FileHelpers
{
    public static class YoutubeHelpers
    {
        private static object s_youtubeLock = new object();
        public static YoutubeSettings GetYoutubeSettings()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/youtube_settings.json";
            lock (s_youtubeLock)
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        return JsonConvert.DeserializeObject<YoutubeSettings>(sr.ReadToEnd());
                    }
                }
            }
        }
    }
}
