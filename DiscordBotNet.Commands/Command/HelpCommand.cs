using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.DiscordWrappers;

namespace DiscordBotNet.Module.Command
{
    public class HelpCommand : BaseCommand
    {
        public HelpCommand() : base("help", "Help", "Displays helpful information about the module")
        {
        }

        public override bool Execute(ISendWrapper sender)
        {
            if (!base.Execute(sender))
            {
                return false;
            }

            foreach (var command in Module.Commands)
            {
                var sb = new StringBuilder();
                if (string.IsNullOrEmpty(command.Prefix))
                {
                    sb.Append($"{command.Name} - invoke using \"{Module.Manager.Prefix}{Module.Prefix}\"");
                }
                else
                {
                    sb.Append($"{command.Name} - invoke using \"{Module.Manager.Prefix}{Module.Prefix} {Module.Manager.Prefix}{command.Prefix}\"");
                }

                sb.Append($" - {command.Description}");
                
                sender.SendMessage(sb.ToString(), Module.Manager.DiscordSettings.IsTTS);
            }

            return true;
        }
    }
}
