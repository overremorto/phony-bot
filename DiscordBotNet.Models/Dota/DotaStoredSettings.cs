using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotNet.Models.Dota
{
    public class DotaStoredSettings
    {
        public long LastMatchId { get; set; }

        public bool SentMessageForLastMatch { get; set; }

        public bool DidWinLastMatch { get; set; }
        public string LastMatchHighestKDA { get; set; }
        public string LastMatchHighestKills { get; set; }
        public string LastMatchHighestGPM { get; set; }
    }
}
