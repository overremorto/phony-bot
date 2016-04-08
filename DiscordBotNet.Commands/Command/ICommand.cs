using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.DiscordWrappers;

namespace DiscordBotNet.Module.Command
{
    public interface ICommand
    {
        Module.IModule Module { get; set; }
        string Prefix { get; }
        string Name { get; }
        string Description { get; }
        bool Execute(ISendWrapper sender);
    }
}
