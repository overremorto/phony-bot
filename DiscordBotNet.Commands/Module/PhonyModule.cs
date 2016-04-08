using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models;
using DiscordBotNet.Module.Command;

namespace DiscordBotNet.Module.Module
{
    public class PhonyModule : BaseModule
    {
        public PhonyModule() : base($"{Consts.BotPrefix}", $"{Consts.BotName}", $"Chat with {Consts.BotName}")
        {
            AddCommand(new ChatCommand());
        }
    }
}
