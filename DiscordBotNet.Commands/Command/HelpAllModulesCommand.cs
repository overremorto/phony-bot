using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.DiscordWrappers;

namespace DiscordBotNet.Module.Command
{
    public class HelpAllModulesCommand : BaseCommand
    {
        public HelpAllModulesCommand() : base("", "Help All", "Shows all installed modules")
        {
        }


        public override bool Execute(ISendWrapper sender)
        {
            foreach (var module in Module.Manager.Modules)
            {
                sender.SendMessage($"{module.Name} - invoke using \"{module.Manager.Prefix}{module.Prefix}\" - {module.Description}");
            }
            return true;
        }
    }
}
