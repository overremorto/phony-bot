using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.FileHelpers;
using DiscordBotNet.Models;
using DiscordBotNet.Models.ChatBot;
using DiscordBotNet.Models.DiscordWrappers;

namespace DiscordBotNet.Module.Command
{
    public class ChatCommand : BaseCommand
    {
        public ChatCommand() : base("", $"Chat with {Consts.BotName}", $"Have friendly chat with {Consts.BotName}, the only wizard bot in the entire channel!")
        {

        }

        public override bool Execute(ISendWrapper sender)
        {
            var httpClient = new HttpClient();
            var message = sender.RemainingMessage;
            var userName = sender.Author.Username;
            var chatBotUserId = ChatBotHelpers.GetChatBotUserId(sender.Author.ID);
            var chatBotSettings = ChatBotHelpers.GetChatBotSettings();
            var url = $"http://www.personalityforge.com/api/chat/?apiKey={chatBotSettings.ApiKey}&chatBotID={chatBotSettings.ChatBotId}&message={WebUtility.UrlEncode(message)}&externalID={chatBotUserId}&firstName={userName}&gender=m";

            var task = httpClient.GetStringAsync(url);

            task.Wait();
            try
            {
                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageResponse>(task.Result);
                sender.SendMessage(response.GetMessageText(), Module.Manager.DiscordSettings.IsTTS);
            }
            catch (Exception ex)
            {
                sender.SendMessage(task.Result, Module.Manager.DiscordSettings.IsTTS);
            }

            return true;
        }
    }
}
