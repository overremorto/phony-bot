using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotNet.Models
{
    public static class Consts
    {
#if DEBUG
        public static string BotName { get { return "phony-bot-dev"; } }
        public static string BotPrefix { get { return "dev"; } }
#else

        public static string BotName { get { return "phony-bot"; } }
        public static string BotPrefix { get { return "phony"; } }
#endif
    }
}
