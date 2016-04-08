using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models;
using DiscordBotNet.Module.Command;

namespace DiscordBotNet.Module.Module
{
    public class PhonyDotaModule : BaseModule
    {
        public PhonyDotaModule() : base($"{Consts.BotPrefix}-dota", $"{Consts.BotName} Dota", "Displays dota related information.")
        {
            AddCommand(new DotaLastMatchCommand());
            AddCommand(new DotaScanCommand());
        }
    }
}
