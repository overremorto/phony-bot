using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.DiscordWrappers;
using NAudio.Wave;

namespace DiscordBotNet.Module.Command
{
    public class CenaCommand : BaseVoiceCommand
    {
        public CenaCommand() : base("cena", "", "")
        {
        }

        public override bool Execute(ISendWrapper sender)
        {
            if (!base.Execute(sender))
            {
                return false;
            }

            if (VoiceModule != null)
            {
                if (VoiceModule.CurrentVoiceChannel == null)
                {
                    var cmd = new JoinVoiceCommand();
                    sender.SendMessage($"Please join a channel first with {Module.Manager.Prefix}{Module.Prefix} {Module.Manager.Prefix}{cmd.Prefix}");
                }
                else
                {
                    int ms = 60;
                    int channels = 1;
                    int sampleRate = 48000;

                    int blockSize = 48 * 2 * channels * ms; //sample rate * 2 * channels * milliseconds
                    byte[] buffer = new byte[blockSize];
                    var outFormat = new WaveFormat(sampleRate, 16, channels);

                    var vc = sender.DiscordClient.GetVoiceClient();
                    vc.SetSpeaking(true);
                    using (var mp3Reader = new MediaFoundationReader("C:\\Users\\Trevor\\Music\\cena-short.mp3"))
                    {
                        using (var resampler = new MediaFoundationResampler(mp3Reader, outFormat) { ResamplerQuality = 60 })
                        {
                            //resampler.ResamplerQuality = 60;
                            int byteCount;
                            while ((byteCount = resampler.Read(buffer, 0, blockSize)) > 0)
                            {
                                if (vc.Connected)
                                {
                                    vc.SendVoice(buffer);
                                }
                                else
                                    break;
                            }
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Voice finished enqueuing");
                            Console.ForegroundColor = ConsoleColor.White;
                            resampler.Dispose();
                            mp3Reader.Close();
                        }
                    }
                }
            }

            return true;
        }
    }
}
