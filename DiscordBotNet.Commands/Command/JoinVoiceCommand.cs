using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models;
using DiscordBotNet.Models.DiscordWrappers;

namespace DiscordBotNet.Module.Command
{
    public class JoinVoiceCommand : BaseVoiceCommand
    {
        public JoinVoiceCommand() : base("join", "Join Voice Channel", $"Makes {Consts.BotName} join a voice channel")
        {
        }

        protected override string PrefixToSearch
        {
            get
            {
                return base.PrefixToSearch + " ";
            }
        }

        public override bool Execute(ISendWrapper sender)
        {
            if (!base.Execute(sender))
            {
                return false;
            }

            var channelSender = sender as ChannelSendWrapper;
            if (channelSender == null)
            {
                return true;
            }

            sender.RemainingMessage = sender.RemainingMessage.Substring(PrefixToSearch.Length);
            var channel = channelSender.Channel.Parent.Channels.FirstOrDefault(c => c.Type == DiscordSharp.Objects.ChannelType.Voice && c.Name.Equals(sender.RemainingMessage, StringComparison.InvariantCultureIgnoreCase));
            if (VoiceModule != null)
            {
                if (channel != null)
                {
                    sender.SendMessage($"Joining channel: {sender.RemainingMessage}");
                    VoiceModule.SetCurrentVoiceChannel(channel, sender.DiscordClient);
                    sender.DiscordClient.GetVoiceClient().GetDebugLogger.EnableLogging = true;
                    sender.DiscordClient.GetVoiceClient().GetDebugLogger.LogMessageReceived += GetDebugLogger_LogMessageReceived;
                    
                }
                else
                {
                    sender.SendMessage($"Unable to find voice channel: {sender.RemainingMessage}");
                }
            }

            return true;
        }

        private void GetDebugLogger_LogMessageReceived(object sender, DiscordSharp.LoggerMessageReceivedArgs e)
        {
            Console.WriteLine(e.message);
        }
    }
}
