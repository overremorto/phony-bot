using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.FileHelpers;
using DiscordBotNet.Models;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Models.Game;

namespace DiscordBotNet.Module.Command.Game
{
    public class StartCommand : BaseGameCommand
    {
        public StartCommand() : base("start", $"Start Game", $"Start a game with {Consts.BotName}!")
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

            try
            {
                sender.RemainingMessage = sender.RemainingMessage.Substring(PrefixToSearch.Length);
                var games = WebAdventureHelpers.GameList();
                var game = games.FirstOrDefault(g => g.Title.StartsWith(sender.RemainingMessage, StringComparison.InvariantCultureIgnoreCase));

                if (game != null)
                {
                    if (GameModule.ActiveGames.ContainsKey(sender.Author.ID))
                    {
                        sender.SendMessage("You are already in a game, please quit before starting a new one", Module.Manager.DiscordSettings.IsTTS);
                        return true;
                    }
                    GameResponse gameResponse = null;
                    var sessionId = GameModule.GetUserCurrentSessionForGame(game.Id, sender.Author.ID);
                    if (!string.IsNullOrEmpty(sessionId))
                    {
                        sender.SendMessage($"Resuming game: {game.Title}", Module.Manager.DiscordSettings.IsTTS);
                        gameResponse = WebAdventureHelpers.LoadGame(game, sessionId);
                    }
                    else
                    {
                        sender.SendMessage($"Starting game: {game.Title}", Module.Manager.DiscordSettings.IsTTS);
                        gameResponse = WebAdventureHelpers.NewGame(game);
                    }


                    
                    foreach (var message in gameResponse.Text)
                    {
                        sender.SendMessage(message);
                    }

                    GameModule.SaveGameSession(game.Id, sender.Author.ID, gameResponse.SessionId);
                    GameModule.SetPlayerActiveGame(sender.Author.ID, new ActiveGameModel(game, gameResponse.SessionId));
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

            return true;
        }
    }
}
