using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Module.Module;
using DiscordSharp.Objects;

namespace DiscordBotNet.Module.ModuleManager
{
    public class ModuleManager : IModuleManager
    {
        public ModuleManager(DiscordSettings settings)
        {
            Modules = new List<IModule>();
            DiscordSettings = settings;
            Prefix = settings.Prefix;
        }

        public string Prefix { get; private set; }

        public DiscordSettings DiscordSettings { get; private set; }

        public DiscordChannel CurrentChannel { get; set; }

        public List<IModule> Modules { get; set; }


        public void HandleRequest(ISendWrapper sender)
        {
            if (sender == null || sender.Author.Username == $"{Consts.BotName}")
            {
                return;
            }
            if (CurrentChannel == null && sender is ChannelSendWrapper)
            {
                CurrentChannel = ((ChannelSendWrapper)sender).Channel;
            }

            sender.RemainingMessage = sender.FullMessage;

            if (sender.RemainingMessage?.StartsWith(Prefix) ?? false)
            {
                sender.RemainingMessage = sender.RemainingMessage.Substring(1);
                foreach (var module in Modules)
                {
                    if (module.ExecuteModule(sender))
                    {
                        break;
                    }
                }
            }
        }

        public void Init()
        {
            foreach (var module in Modules)
            {
                module.Init();
            }
        }

        public void InstallModule(IModule module)
        {
            module.Install(this);
        }

        public void RunContinuous()
        {
            foreach (var module in Modules)
            {
                module.RunContinuous();
            }
        }
    }
}
