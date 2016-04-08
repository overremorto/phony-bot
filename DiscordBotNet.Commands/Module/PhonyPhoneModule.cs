using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Module.Command;

namespace DiscordBotNet.Module.Module
{
    public class PhonyPhoneModule : BaseModule
    {
        public PhonyPhoneModule() : base("phone", "Phony Phone", "Use the phony command not the phone command")
        {
            AddCommand(new PhoneCommand());
        }
    }
}
