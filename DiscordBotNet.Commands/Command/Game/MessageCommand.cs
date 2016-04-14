using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.FileHelpers;
using DiscordBotNet.Models;
using DiscordBotNet.Models.DiscordWrappers;

namespace DiscordBotNet.Module.Command.Game
{
    public class MessageCommand : BaseGameCommand
    {
        public MessageCommand() : base("start", $"Start a Game", $"Play the game with {Consts.BotName}!")
        {
        }

        public override bool Execute(ISendWrapper sender)
        {
            try
            {
                // no active game
                if (!GameModule.ActiveGames.ContainsKey(sender.Author.ID))
                {
                    return false;
                }

                var response = WebAdventureHelpers.SendMessageToGame(GameModule.ActiveGames[sender.Author.ID], sender.RemainingMessage);

                foreach (var text in response.Text)
                {
                    sender.SendMessage(text, Module.Manager.DiscordSettings.IsTTS);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return true;
        }
    }
}
