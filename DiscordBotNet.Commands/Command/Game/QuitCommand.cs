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
    public class QuitCommand : BaseGameCommand
    {
        public QuitCommand() : base("quit", $"Quit Game", $"Quit the current game you are playing")
        {
        }

        public override bool Execute(ISendWrapper sender)
        {
            if (!base.Execute(sender))
            {
                return false;
            }

            if (!GameModule.ActiveGames.ContainsKey(sender.Author.ID))
            {
                sender.SendMessage("You are not in a game.", Module.Manager.DiscordSettings.IsTTS);
                return true;
            }
            try
            {
                var games = WebAdventureHelpers.GameList();
                var game = games.FirstOrDefault(g => g.Id == GameModule.ActiveGames[sender.Author.ID]?.Game.Id);
                if (game != null)
                {
                    sender.SendMessage($"Quiting game: {game.Title}", Module.Manager.DiscordSettings.IsTTS);
                }
                else
                {
                    sender.SendMessage("Game not found", Module.Manager.DiscordSettings.IsTTS);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                GameModule.ActiveGames.Remove(sender.Author.ID);
            }
            return true;
        }
    }
}
