using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models;
using DiscordBotNet.Models.DiscordWrappers;

namespace DiscordBotNet.Module.Command
{
    public class LeaveVoiceCommand : BaseVoiceCommand
    {
        public LeaveVoiceCommand() : base("leave", "Leave Voice Channel", $"Makes {Consts.BotName} leave the voice channel.")
        {
        }

        public override bool Execute(ISendWrapper sender)
        {
            if (!base.Execute(sender))
            {
                return false;
            }

            if (VoiceModule != null && VoiceModule.CurrentVoiceChannel != null)
            {
                sender.SendMessage($"Leaving voice channel: {VoiceModule.CurrentVoiceChannel.Name}");
                VoiceModule.SetCurrentVoiceChannel(null, sender.DiscordClient);
            }
            else
            {
                sender.SendMessage("Not connected to a voice channel");
            }


            return true;
        }
    }
}
