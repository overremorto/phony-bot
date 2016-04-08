using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.FileHelpers;
using DiscordBotNet.Models;
using DiscordBotNet.Models.DiscordWrappers;

namespace DiscordBotNet.Module.Command
{
    public class VoiceChangeCommand : BaseCommand
    {
        public VoiceChangeCommand() : base("change", "Change Voice", $"Changes the voice for {Consts.BotName}")
        {
        }

        protected override string PrefixToSearch
        {
            get
            {
                return base.PrefixToSearch + " ";
            }
        }

        public override bool Execute(ISendWrapper sender)
        {
            if (!base.Execute(sender))
            {
                return false;
            }


            sender.RemainingMessage = sender.RemainingMessage.StartsWith(PrefixToSearch) ? 
                sender.RemainingMessage.Substring(PrefixToSearch.Length) : 
                sender.RemainingMessage.Substring(PrefixToSearch.Trim().Length);
            string message = string.Empty;
            var currentSettings = VoiceHelpers.GetVoiceSettings();

            if (currentSettings.AvailableVoices.Any(v => v.Equals(sender.RemainingMessage, StringComparison.CurrentCultureIgnoreCase)))
            {
                currentSettings.CurrentVoice = currentSettings.AvailableVoices.FirstOrDefault(v => v.Equals(sender.RemainingMessage, StringComparison.CurrentCultureIgnoreCase));
                message = $"Changing voice to {currentSettings.CurrentVoice}";
                VoiceHelpers.SaveVoiceSettings(currentSettings);
            }
            else if (currentSettings.AvailableVoices.Any(v => v.StartsWith(sender.RemainingMessage, StringComparison.CurrentCultureIgnoreCase)) && !string.IsNullOrWhiteSpace(sender.RemainingMessage))
            {
                currentSettings.CurrentVoice = currentSettings.AvailableVoices.FirstOrDefault(v => v.StartsWith(sender.RemainingMessage, StringComparison.CurrentCultureIgnoreCase));
                message = $"Changing voice to {currentSettings.CurrentVoice}";
                VoiceHelpers.SaveVoiceSettings(currentSettings);
            }
            else
            {
                if (!string.IsNullOrEmpty(sender.RemainingMessage))
                {
                    sender.SendMessage($"Voice not found: {sender.RemainingMessage}");
                }

                message = $"These are the supported voices: {string.Join(", ", currentSettings.AvailableVoices)}";
            }
            sender.SendMessage(message);

            return true;
        }
    }
}
