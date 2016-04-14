using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotNet.Models.Game
{
    public class ActiveGameModel
    {
        public ActiveGameModel(GameModel game, string sessionId)
        {
            Game = game;
            SessionId = sessionId;
        }

        public GameModel Game { get; set; }

        public string SessionId { get; set; }
    }
}
