using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotNet.Models.Game
{
    public class GameResponse
    {
        public IEnumerable<string> Text { get; set; }

        public string SessionId { get; set; }
    }
}
