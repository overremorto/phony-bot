using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.Dota;
using Newtonsoft.Json;

namespace DiscordBotNet.FileHelpers
{
    public static class DotaFileHelpers
    {
        private static object s_dotaFileLock = new object();
        public static DotaStoredSettings GetDotaStoredSettings()
        {
            DotaStoredSettings settings = null;
            var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/dota_api_storage.json";
            lock (s_dotaFileLock)
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        settings = JsonConvert.DeserializeObject<DotaStoredSettings>(sr.ReadToEnd());
                    }
                }
            }

            if (settings == null)
            {
                settings = new DotaStoredSettings();
                SetDotaStoredSettings(settings);
            }

            return settings;
        }

        public static DotaSettings GetDotaSettings()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/dota_settings.json";
            lock (s_dotaFileLock)
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        return JsonConvert.DeserializeObject<DotaSettings>(sr.ReadToEnd());
                    }
                }
            }
        }

        public static void SetDotaStoredSettings(DotaStoredSettings settings)
        {
            lock (s_dotaFileLock)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/dota_api_storage.json";
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(JsonConvert.SerializeObject(settings));
                    }
                }
            }
        }

        public static Dictionary<string, DotaPlayerStorage> GetDotaPlayers()
        {
            Dictionary<string, DotaPlayerStorage> players = null;
            var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/dota_player_storage.json";
            lock (s_dotaFileLock)
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        players = JsonConvert.DeserializeObject<Dictionary<string, DotaPlayerStorage>>(sr.ReadToEnd());
                    }
                }
            }

            if (players == null)
            {
                players = new Dictionary<string, DotaPlayerStorage>();
            }

            return players;
        }
    }
}
