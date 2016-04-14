using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Module.Module;

namespace DiscordBotNet.Module.Command.Game
{
    public class BaseGameCommand : BaseCommand
    {
        public PhonyGameModule GameModule
        {
            get
            {
                return Module as PhonyGameModule;
            }
        }

        public BaseGameCommand(string prefix, string name, string description) : base(prefix, name, description)
        {
        }
    }
}
