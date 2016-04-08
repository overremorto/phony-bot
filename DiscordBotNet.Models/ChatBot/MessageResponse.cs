using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotNet.Models.ChatBot
{
    public class MessageResponse
    {
        public string ErrorMessage { get; set; }

        public int Success { get; set; }

        public MessageMessage Message { get; set; }

        public string GetMessageText()
        {
            var message = Message?.Message ?? string.Empty;
            var chatBotName = Message?.ChatBotName ?? null;
            while (!string.IsNullOrWhiteSpace(chatBotName) && message.Contains(chatBotName))
            {
                message = message.Replace(chatBotName, Consts.BotName);
            }

            return message;
        }
    }

    public class MessageMessage
    {

        public string ChatBotID { get; set; }

        public string ChatBotName { get; set; }

        public string Emotion { get; set; }

        public string Message { get; set; }
    }
}
