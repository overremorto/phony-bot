using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models;
using DiscordBotNet.Module.Command;

namespace DiscordBotNet.Module.Module
{
    public class HelpModule : BaseModule
    {
        public HelpModule() : base($"{Consts.BotPrefix}-help", "Help", "Displays the documentation for the other modules.")
        {
            AddCommand(new HelpCommand());
            AddCommand(new HelpAllModulesCommand());
        }

        public override void Init()
        {
            base.Init();

            foreach (var module in Manager.Modules)
            {
                if (module != this)
                {
                    module.InsertCommand(0, new HelpCommand());
                }
            }
        }
    }
}
