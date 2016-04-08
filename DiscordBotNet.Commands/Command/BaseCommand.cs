using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Module.Module;

namespace DiscordBotNet.Module.Command
{
    public abstract class BaseCommand : ICommand
    {
        public string Prefix { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        private IModule _module;
        public IModule Module { get { return _module; } set { _module = _module ?? value; } }

        protected virtual string PrefixToSearch
        {
            get
            {
                return Module.Manager.Prefix + Prefix;
            }
        }

        public BaseCommand(string prefix, string name, string description)
        {
            Prefix = prefix;
            Name = name;
            Description = description;
        }



        public virtual bool Execute(ISendWrapper sender)
        {
            if (!sender.RemainingMessage.StartsWith(PrefixToSearch) && !sender.RemainingMessage.StartsWith(PrefixToSearch.Trim()))
            {
                return false;
            }

            //sender.RemainingMessage = sender.RemainingMessage.Substring(PrefixToSearch.Length);

            return true;
        }
    }
}
