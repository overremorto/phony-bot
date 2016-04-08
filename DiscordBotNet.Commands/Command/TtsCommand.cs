using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Module.Module;

namespace DiscordBotNet.Module.Command
{
    public class TtsCommand : BaseCommand
    {
        public TtsCommand() : base("", "TTS", "Toggles TTS (Text-to-Speach)")
        {
        }

        public override bool Execute(ISendWrapper sender)
        {
            Module.Manager.DiscordSettings.IsTTS = !Module.Manager.DiscordSettings.IsTTS;
            sender.SendMessage($"TTS (Text-to-Speach) is {(Module.Manager.DiscordSettings.IsTTS ? "enabled" : "disabled")}", Module.Manager.DiscordSettings.IsTTS);
            return true;
        }
    }
}
