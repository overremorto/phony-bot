using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models;
using DiscordBotNet.Module.Command;

namespace DiscordBotNet.Module.Module
{
    public class PhonyMathModule : BaseModule
    {
        public PhonyMathModule() : base($"{Consts.BotPrefix}-math", $"{Consts.BotName} Math", "Calculator to help with math issues.")
        {
            AddCommand(new MathCommand());
        }
    }
}
