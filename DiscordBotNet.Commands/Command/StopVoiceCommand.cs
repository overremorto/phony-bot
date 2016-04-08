using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models;
using DiscordBotNet.Models.DiscordWrappers;

namespace DiscordBotNet.Module.Command
{
    public class StopVoiceCommand : BaseVoiceCommand
    {
        public StopVoiceCommand() : base("stop", "Stop Voice", $"Makes {Consts.BotName} stop talking")
        {
        }

        public override bool Execute(ISendWrapper sender)
        {
            if (!base.Execute(sender))
            {
                return false;
            }

            if (VoiceModule.CurrentVoiceChannel == null)
            {
                var cmd = new JoinVoiceCommand();
                sender.SendMessage($"Please join a channel first with {Module.Manager.Prefix}{Module.Prefix} {Module.Manager.Prefix}{cmd.Prefix}");
            }
            else
            {
                var vc = sender.DiscordClient.GetVoiceClient();
                if (vc.Connected)
                {
                    VoiceModule.IsStop = true;
                    vc.ClearVoiceQueue();
                    sender.SendMessage("Sorry, shutting up now...");
                }
            }

            return true;
        }
    }
}
