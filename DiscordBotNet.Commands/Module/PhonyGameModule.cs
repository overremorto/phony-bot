using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models;
using DiscordBotNet.Models.Game;
using DiscordBotNet.Module.Command.Game;
using DiscordSharp.Objects;

namespace DiscordBotNet.Module.Module
{
    public class PhonyGameModule : BaseModule
    {
        private static object s_SaveGameSession_lock = new object();
        public Dictionary<string, Dictionary<string, string>> SavedGames { get; private set; }

        public Dictionary<string, ActiveGameModel> ActiveGames { get; private set; }

        public PhonyGameModule() : base($"{Consts.BotPrefix}-game", $"{Consts.BotName} Game", $"Play a game with {Consts.BotName}!", true)
        {
            SavedGames = FileHelpers.FileHelper.Get<Dictionary<string, Dictionary<string, string>>>("game_storage") ?? new Dictionary<string, Dictionary<string, string>>();
            ActiveGames = new Dictionary<string, ActiveGameModel>();
            AddCommand(new ListCommand());
            AddCommand(new StartCommand());
            AddCommand(new QuitCommand());
            AddCommand(new MessageCommand());
        }

        public void SaveGameSession(string game, string userId, string sessionId)
        {
            lock (s_SaveGameSession_lock)
            {
                if (!SavedGames.ContainsKey(game))
                {
                    SavedGames[game] = new Dictionary<string, string>();
                }

                var gameSaveFiles = SavedGames[game];
                gameSaveFiles[userId] = sessionId;

                FileHelpers.FileHelper.SaveJson("game_storage", SavedGames);
            }
        }

        public string GetUserCurrentSessionForGame(string gameId, string userId)
        {
            return SavedGames.ContainsKey(gameId) && SavedGames[gameId].ContainsKey(userId) ? SavedGames[gameId][userId] : null;
        }

        public void SetPlayerActiveGame(string userId, ActiveGameModel game)
        {
            ActiveGames[userId] = game;
        }
    }
}
