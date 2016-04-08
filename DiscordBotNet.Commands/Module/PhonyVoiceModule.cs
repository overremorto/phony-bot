using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models;
using DiscordBotNet.Module.Command;
using DiscordSharp;
using DiscordSharp.Objects;

namespace DiscordBotNet.Module.Module
{
    public class PhonyVoiceModule : BaseModule
    {
        public bool IsStop { get; set; }

        public PhonyVoiceModule() : base($"{Consts.BotPrefix}-voice", "Phony Voice", $"Makes {Consts.BotName} speak in the voice channel")
        {
            AddCommand(new JoinVoiceCommand());
            AddCommand(new LeaveVoiceCommand());
            AddCommand(new MusicCommand());
            AddCommand(new StopVoiceCommand());
            AddCommand(new VoiceChangeCommand());
            AddCommand(new SingCommand());
            AddCommand(new VoiceCommand());
        }

        public void SetCurrentVoiceChannel(DiscordChannel channel, DiscordClient discordClient)
        {
            if (channel == null)
            {
                if (CurrentVoiceChannel != null)
                {
                    discordClient.DisconnectFromVoice();
                }
            }
            else if (channel.Type == ChannelType.Voice)
            {
                CurrentVoiceChannel = channel;
                discordClient.ConnectToVoiceChannel(channel, new DiscordVoiceConfig() { Bitrate = null, Channels = 1, FrameLengthMs = 60, OpusMode = DiscordSharp.Voice.OpusApplication.LowLatency, SendOnly = true });
                
            }
        }

        public DiscordChannel CurrentVoiceChannel { get; private set; }
    }
}
