using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.Voice;
using Newtonsoft.Json;

namespace DiscordBotNet.FileHelpers
{
    public static class VoiceHelpers
    {
        private static object s_voiceLock = new object();
        public static VoiceSettings GetVoiceSettings()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/voice_settings.json";
            lock (s_voiceLock)
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        return JsonConvert.DeserializeObject<VoiceSettings>(sr.ReadToEnd());
                    }
                }
            }
        }


        public static void SaveVoiceSettings(VoiceSettings settings)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/voice_settings.json";
            lock (s_voiceLock)
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    fs.SetLength(0);
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(JsonConvert.SerializeObject(settings));
                    }
                }
            }
        }
    }
}
