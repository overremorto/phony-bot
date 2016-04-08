using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordSharp;
using DiscordSharp.Objects;

namespace DiscordBotNet.Models.DiscordWrappers
{
    public class ChannelSendWrapper : ISendWrapper
    {
        public DiscordChannel Channel;
        public ChannelSendWrapper(DiscordChannel channel, string fullMessage, DiscordMember author, DiscordClient discordClient)
        {
            Channel = channel;
            FullMessage = fullMessage;
            Author = author;
            DiscordClient = discordClient;
        }
        public DiscordMember Author { get; set; }
        public string FullMessage { get; set; }

        public string RemainingMessage { get; set; }

        public void SendMessage(string message, bool tts)
        {
            Channel.SendMessage(message);
        }

        public DiscordClient DiscordClient { get; private set; }
    }
}
