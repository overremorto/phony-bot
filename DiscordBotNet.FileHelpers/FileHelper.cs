using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBotNet.FileHelpers
{
    public static class FileHelper
    {
        private static object s_fileLock_lock = new object();
        public static void SaveJson<T>(string file, T obj)
        {

            lock (s_fileLock_lock)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + $"/persistentData/{file}.json";
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(JsonConvert.SerializeObject(obj));
                    }
                }
            }
        }

        public static T Get<T>(string file)
            where T : class
        {
            var result = default(T);
            var path = AppDomain.CurrentDomain.BaseDirectory + $"/persistentData/{file}.json";
            lock (s_fileLock_lock)
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        result = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
                    }
                }
            }

            if (result == null)
            {
                result = Activator.CreateInstance<T>();
            }

            return result;
        }
    }
}
