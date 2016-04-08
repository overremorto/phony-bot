using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.DiscordWrappers;
using Newtonsoft.Json;

namespace DiscordBotNet.FileHelpers
{
    public class DiscordHelpers
    {
        private static object s_discordLock = new object();
        public static DiscordLoginSettings GetDiscordSettings()
        {
#if DEBUG
            var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/discord_settings_dev.json";
#else

            var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/discord_settings.json";
#endif
        
            lock (s_discordLock)
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        return JsonConvert.DeserializeObject<DiscordLoginSettings>(sr.ReadToEnd());

                    }
                }
            }
        }
    }
}
