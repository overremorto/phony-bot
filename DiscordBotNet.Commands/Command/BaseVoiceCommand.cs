using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Module.Module;

namespace DiscordBotNet.Module.Command
{
    public abstract class BaseVoiceCommand : BaseCommand
    {
        public BaseVoiceCommand(string prefix, string name, string description) : base(prefix, name, description)
        {
        }

        protected PhonyVoiceModule VoiceModule
        {
            get
            {
                return Module as PhonyVoiceModule;
            }
        }
    }
}
