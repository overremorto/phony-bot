using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.FileHelpers;
using DiscordBotNet.Models;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Module.Module;
using NAudio.FileFormats.Mp3;
using NAudio.Wave;
using NLayer.NAudioSupport;

namespace DiscordBotNet.Module.Command
{
    public class VoiceCommand : BaseVoiceCommand
    {
        public VoiceCommand() : base("", "Voice", $"Makes {Consts.BotName} speak in the voice channel")
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
            var channelSender = sender as ChannelSendWrapper;
            if (channelSender == null)
            {
                return false;
            }
            try
            {
                if (VoiceModule != null)
                {
                    if (VoiceModule.CurrentVoiceChannel == null)
                    {
                        var cmd = new JoinVoiceCommand();
                        sender.SendMessage($"Please join a channel first with {Module.Manager.Prefix}{Module.Prefix} {Module.Manager.Prefix}{cmd.Prefix}");
                    }
                    else
                    {
                        VoiceModule.IsStop = false;
                        var httpClient = new HttpClient();
                        var parameters = new Dictionary<string, string>();
                        var voiceSettings = VoiceHelpers.GetVoiceSettings();
                        parameters["MyLanguages"] = "sonid10";
                        parameters["MySelectedVoice"] = voiceSettings.CurrentVoice;
                        parameters["MyTextForTTS"] = sender.RemainingMessage;
                        parameters["t"] = "1";
                        parameters["SendToVaaS"] = "";
                        var content = new FormUrlEncodedContent(parameters);
                        var getLinkTask = httpClient.PostAsync("http://www.acapela-group.com/demo-tts/DemoHTML5Form_V2.php?langdemo=Powered+by+%3Ca+href%3D%22http%3A%2F%2Fwww.acapela-vaas.com%22%3EAcapela+Voice+as+a+Service%3C%2Fa%3E.+For+demo+and+evaluation+purpose+only%2C+for+commercial+use+of+generated+sound+files+please+go+to+%3Ca+href%3D%22http%3A%2F%2Fwww.acapela-box.com%22%3Ewww.acapela-box.com%3C%2Fa%3E", content);
                        getLinkTask.Wait();
                        var htmlTask = getLinkTask.Result.Content.ReadAsStringAsync();
                        htmlTask.Wait();
                        var link = HtmlHelper.JavascriptSearch(htmlTask.Result, "myPhpVar", typeof(string));
                        var soundTask = httpClient.GetStreamAsync(link);
                        soundTask.Wait();

                        var soundStream = StreamHelpers.CopyToMemoryStream(soundTask.Result);

                        int ms = 60;
                        int channels = 1;
                        int sampleRate = 48000;

                        int blockSize = 48 * 2 * channels * ms; //sample rate * 2 * channels * milliseconds
                        byte[] buffer = new byte[blockSize];
                        var outFormat = new WaveFormat(sampleRate, 16, channels);

                        var vc = sender.DiscordClient.GetVoiceClient();
                        vc.SetSpeaking(true);
                        using (var mp3Reader = new Mp3FileReader(soundStream, wave => new AcmMp3FrameDecompressor(wave)))
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return true;
        }

    }
}
