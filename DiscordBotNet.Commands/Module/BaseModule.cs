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
    public abstract class BaseModule : IModule
    {
        public List<ICommand> Commands { get; private set; }

        public IModuleManager Manager { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string Prefix { get; private set; }

        public bool ReadCommandless { get; private set; }

        public string PrefixToSearch
        {
            get
            {
                return (Prefix?.ToLower() ?? string.Empty) + " ";
            }
        }

        public BaseModule(string prefix, string name, string description, bool readCommandless = false)
        {
            Prefix = prefix;
            Name = name;
            Description = description;
            ReadCommandless = readCommandless;
            Commands = new List<ICommand>();
        }

        public virtual bool ExecuteModule(ISendWrapper sender)
        {
            if (ReadCommandless && !sender.RemainingMessage.StartsWith(Prefix))
            {
                // skip
            }
            else
            {
                var prefix = PrefixToSearch.Length > sender.RemainingMessage.Length ? Prefix.ToLower() : PrefixToSearch;
                if (!(sender.RemainingMessage?.ToLower().StartsWith(prefix) ?? false))
                {
                    return false;
                }

                sender.RemainingMessage = sender.RemainingMessage.Substring(prefix.Length);
            }

            var wasCommandFound = false;
            foreach (var command in Commands)
            {
                if (command.Execute(sender))
                {
                    wasCommandFound = true;
                    break;
                }
            }

            if (!wasCommandFound)
            {
                CommandNotFound(sender);
            }

            return true;
        }

        public virtual void Init()
        {

        }

        public virtual void CommandNotFound(ISendWrapper sender)
        {
            sender.SendMessage("UNKOWN COMMAND");
        }

        public virtual void Install(IModuleManager manager)
        {
            Manager = manager;
            Manager.Modules.Add(this);
        }

        public void AddCommand(ICommand command)
        {
            command.Module = this;
            Commands.Add(command);
        }

        public virtual void RunContinuous()
        {
        }

        public void InsertCommand(int position, ICommand command)
        {
            command.Module = this;
            Commands.Insert(position, command);
        }
    }
}
