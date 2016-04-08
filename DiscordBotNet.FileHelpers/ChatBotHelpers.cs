using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.ChatBot;
using Newtonsoft.Json;

namespace DiscordBotNet.FileHelpers
{
    public static class ChatBotHelpers
    {
        private static object s_chatBotLock = new object();
        public static string GetChatBotUserId(string userName)
        {
            Dictionary<string, string> users = null;
            var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/user_chatBotUserID.json";
            lock (s_chatBotLock)
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        users = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());

                    }
                }
            }

            if (users == null)
            {
                users = new Dictionary<string, string>();
            }

            if (!users.ContainsKey(userName))
            {
                users[userName] = userName.Length > 30 ? userName.Substring(0, 30) : userName;
                SaveUserNameChatBotUserId(users);
            }

            return users[userName];
        }

        public static void SaveUserNameChatBotUserId(Dictionary<string, string> users)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/user_chatBotUserID.json";
            lock (s_chatBotLock)
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(JsonConvert.SerializeObject(users));
                    }
                }
            }
        }


        public static ChatBotSettings GetChatBotSettings()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/persistentData/personality_forge_settings.json";
            lock (s_chatBotLock)
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        return JsonConvert.DeserializeObject<ChatBotSettings>(sr.ReadToEnd());

                    }
                }
            }
        }
    }
}
