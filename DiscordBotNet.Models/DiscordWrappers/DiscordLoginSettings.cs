using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBotNet.Models.DiscordWrappers
{
    [JsonObject]
    public class DiscordLoginSettings
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }
    }
}
