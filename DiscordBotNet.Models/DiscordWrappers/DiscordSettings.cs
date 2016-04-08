using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotNet.Models.DiscordWrappers
{
    public class DiscordSettings
    {
        public DiscordSettings(string prefix)
        {
            Prefix = prefix;
        }
        public bool IsTTS { get; set; }

        public string Prefix { get; private set; }

        public string BotName {
            get { return Consts.BotName; }
        }
    }
}
