using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiscordBotNet.FileHelpers;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Models.Youtube;
using NAudio.FileFormats.Mp3;
using NAudio.Wave;
using Newtonsoft.Json;

namespace DiscordBotNet.Module.Command
{
    public class MusicCommand : BaseVoiceCommand
    {
        public MusicCommand() : base("music", "Play Song", "Searches Youtube for the song and plays it")
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
                        sender.RemainingMessage = sender.RemainingMessage.Substring(PrefixToSearch.Length);
                        var youtubeSettings = YoutubeHelpers.GetYoutubeSettings();

                        var httpClient = new HttpClient();
                        var searchYoutubeTask = httpClient.GetStringAsync($"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=1&q={WebUtility.UrlEncode(sender.RemainingMessage)}&type=video&key={youtubeSettings.ApiKey}");
                        searchYoutubeTask.Wait();

                        var youtubeResult = JsonConvert.DeserializeObject<YoutubeResult>(searchYoutubeTask.Result);

                        if (!youtubeResult.Items.Any())
                        {
                            sender.SendMessage($"No results found for: {sender.RemainingMessage}", Module.Manager.DiscordSettings.IsTTS);
                            return true;
                        }

                        var foundSong = youtubeResult.Items.FirstOrDefault();
                        sender.SendMessage($"Loading song: {foundSong.Snippet.Title} - Please wait");

                        Task<string> downloadTask = null;
                        YoutubeInMp3Result result = null;
                        var convertInitiated = false;
                        for (var i = 0; i < 10 && result == null; i++)
                        {
                            try
                            {
                                downloadTask = httpClient.GetStringAsync($"http://www.youtubeinmp3.com/fetch/?format=JSON&video=https://www.youtube.com/watch?v={foundSong.Id.VideoId}");
                                downloadTask.Wait();
                                result = JsonConvert.DeserializeObject<YoutubeInMp3Result>(downloadTask.Result);
                            }
                            catch
                            {
                                try
                                {
                                    // dont continually run the convert process if it has already started
                                    if (!convertInitiated)
                                    {
                                        // parse reponse for redirect url
                                        var content = HtmlHelper.XmlSearch(downloadTask.Result, "content");

                                        // remove the auto start download because we will manually call it
                                        var redirectUrl = content.Substring(content.IndexOf("url=") + "url=".Length).Replace("&autostart=1", "");

                                        // get the download page html
                                        var redirectTask = httpClient.GetStringAsync(redirectUrl);
                                        redirectTask.Wait();
                                        var redirectResponse = redirectTask.Result;

                                        // parse the response to get the download element
                                        var element = HtmlHelper.XmlFindElement(redirectResponse, "id", "download");

                                        // get the href from the element
                                        redirectUrl = "http://www.youtubeinmp3.com/" + HtmlHelper.XmlSearch(element, "href");

                                        // send GET to start the convert process
                                        downloadTask = httpClient.GetStringAsync(redirectUrl);
                                        downloadTask.Wait();

                                        convertInitiated = true;
                                    }
                                }
                                catch { }
                                // wait 5 seconds before trying again
                                Thread.Sleep(5000);
                            }
                        }
                        var downloadSongTask = httpClient.GetStreamAsync(result.Link);
                        downloadSongTask.Wait();
                        var songStream = StreamHelpers.CopyToMemoryStream(downloadSongTask.Result);

                        int ms = 60;
                        int channels = 1;
                        int sampleRate = 48000;

                        int blockSize = 48 * 2 * channels * ms; //sample rate * 2 * channels * milliseconds
                        byte[] buffer = new byte[blockSize];
                        var outFormat = new WaveFormat(sampleRate, 16, channels);

                        var vc = sender.DiscordClient.GetVoiceClient();
                        vc.SetSpeaking(true);

                        using (var mp3Reader = new Mp3FileReader(songStream, wave => new AcmMp3FrameDecompressor(wave)))
                        {
                            using (var resampler = new MediaFoundationResampler(mp3Reader, outFormat) { ResamplerQuality = 60 })
                            {
                                sender.SendMessage($"Song loaded. Now playing song: {foundSong.Snippet.Title}");
                                sender.DiscordClient.UpdateCurrentGame($"{foundSong.Snippet.Title}");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return true;
            }
        }
    }
}
