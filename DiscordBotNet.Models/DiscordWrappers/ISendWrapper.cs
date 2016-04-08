using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordSharp;
using DiscordSharp.Objects;
namespace DiscordBotNet.Models.DiscordWrappers
{
    public interface ISendWrapper
    {
        string FullMessage { get; set; }

        string RemainingMessage { get; set; }

        void SendMessage(string message, bool tts = false);

        DiscordMember Author { get; set; }

        DiscordClient DiscordClient { get; }
    }
}
