using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models;
using DiscordBotNet.Module.Command;

namespace DiscordBotNet.Module.Module
{
    public class PhonyTtsModule : BaseModule
    {
        public PhonyTtsModule() : base($"{Consts.BotPrefix}-tts", $"{Consts.BotName} TTS", "Toggles TTS.")
        {
            AddCommand(new TtsCommand());
        }
    }
}
