using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Module.Module;
using DiscordSharp.Objects;

namespace DiscordBotNet.Module.ModuleManager
{
    public interface IModuleManager
    {
        string Prefix { get; }
        DiscordSettings DiscordSettings { get;}
        DiscordChannel CurrentChannel { get; set; }
        List<IModule> Modules { get; set; }
        void InstallModule(IModule module);

        void HandleRequest(ISendWrapper sender);

        void RunContinuous();

        void Init();
        
    }
}
