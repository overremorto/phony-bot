using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Module.Command;
using DiscordBotNet.Module.ModuleManager;

namespace DiscordBotNet.Module.Module
{
    public interface IModule
    {
        void Init();
        IModuleManager Manager { get; }
        string Prefix { get; }

        string Name { get; }

        string Description { get; }

        List<ICommand> Commands { get; }

        void Install(IModuleManager manager);

        bool ExecuteModule(ISendWrapper sender);
        void AddCommand(ICommand command);
        void InsertCommand(int position, ICommand command);
        void RunContinuous();
    }
}
