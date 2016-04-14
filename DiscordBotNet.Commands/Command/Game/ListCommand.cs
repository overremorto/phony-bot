using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.FileHelpers;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Models.Game;
using Newtonsoft.Json;

namespace DiscordBotNet.Module.Command.Game
{
    public class ListCommand : BaseGameCommand
    {
        public ListCommand() : base("list", "List Games", "Lists all of the available games to play.")
        {
        }

        public override bool Execute(ISendWrapper sender)
        {
            if (!base.Execute(sender))
            {
                return false;
            }


            var games = WebAdventureHelpers.GameList();

            sender.SendMessage(string.Join(", ", games.Select(g => g.Title)), Module.Manager.DiscordSettings.IsTTS);

            return true;
        }
    }
}
