using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordSharp;
using DiscordSharp.Objects;

namespace DiscordBotNet.Models.DiscordWrappers
{
    public class PrivateSendWrapper : ISendWrapper
    {
        public PrivateSendWrapper(DiscordMember member, string fullMessage, DiscordClient discordClient)
        {
            Author = member;
            FullMessage = fullMessage;
            DiscordClient = discordClient;
        }
        public string FullMessage { get; set; }

        public DiscordMember Author { get; set; }

        public string RemainingMessage { get; set; }

        public void SendMessage(string message, bool tts)
        {
            Author.SendMessage(message);
        }
        public DiscordClient DiscordClient { get; private set; }
    }
}
