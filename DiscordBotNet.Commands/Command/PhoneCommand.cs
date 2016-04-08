using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.DiscordWrappers;

namespace DiscordBotNet.Module.Command
{
    public class PhoneCommand : BaseCommand
    {
        public PhoneCommand() : base("", "Phone not Phony", "This is just to remind those that type phone instead of phony")
        {

        }

        public override bool Execute(ISendWrapper sender)
        {
            sender.SendMessage("You probably meant to type phony and not phone...", Module.Manager.DiscordSettings.IsTTS);
            return true;
        }
    }
}
