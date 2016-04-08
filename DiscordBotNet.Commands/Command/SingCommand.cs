using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.FileHelpers;
using DiscordBotNet.Models;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Models.Voice;
using NAudio.Wave;

namespace DiscordBotNet.Module.Command
{
    public class SingCommand : BaseVoiceCommand
    {
        public SingCommand() : base("sing", "Sing", $"Make {Consts.BotName} sing a song")
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
                VoiceModule.IsStop = false;
                sender.RemainingMessage = sender.RemainingMessage.Substring(PrefixToSearch.Length);
                
                var song = GetSongFromMLDB(sender.RemainingMessage);
                // if song couldnt be found from mldb try lyrics.com
                if (song == null)
                {
                    song = GetSongFromLyricsDotCom(sender.RemainingMessage);
                }

                if (song != null)
                {
                    sender.SendMessage($"Singing {song.SongName} by {song.ArtistName}");
                    sender.DiscordClient.UpdateCurrentGame($"{song.SongName} by {song.ArtistName}");
                    SendVoice(sender, song.Lyrics);
                }
                else
                {
                    sender.SendMessage("Song not found");
                }

            }
            catch (Exception ex)
            {

            }

            return true;
        }

        public SongResultModel GetSongFromMLDB(string search)
        {
            SongResultModel song = null;
            try {
                var lyricsUrl = $"http://www.mldb.org/search?mq={WebUtility.UrlEncode(search)}&si=0&mm=0&ob=1";
                var httpClient = new HttpClient();
                var lyricsTask = httpClient.GetStringAsync(lyricsUrl);
                lyricsTask.Wait();
                var lyricsHtml = lyricsTask.Result;
                var indexStart = lyricsHtml.IndexOf("class=\"songtext\"");
                int indexEnd;
                // didnt find an exact match, grab the first item in the list of matches
                if (indexStart == -1)
                {

                    indexStart = lyricsHtml.IndexOf("class=\"ft\"");
                    indexStart = lyricsHtml.IndexOf("href=\"", indexStart) + "href=\"".Length;
                    indexEnd = lyricsHtml.IndexOf("\"", indexStart);
                    var songUrl = "http://www.mldb.org/" + lyricsHtml.Substring(indexStart, indexEnd - indexStart);
                    lyricsTask = httpClient.GetStringAsync(songUrl);
                    lyricsTask.Wait();
                    lyricsHtml = lyricsTask.Result;
                }

                indexStart = lyricsHtml.IndexOf("class=\"centerplane\"");
                indexStart = lyricsHtml.IndexOf("<h1>", indexStart) + "<h1>".Length;
                indexEnd = lyricsHtml.IndexOf("<", indexStart);
                var songName = lyricsHtml.Substring(indexStart, indexEnd - indexStart).Trim();

                indexStart = lyricsHtml.IndexOf("id=\"thelist\"");
                indexStart = lyricsHtml.IndexOf("<a", indexStart);
                indexStart = lyricsHtml.IndexOf(">", indexStart);
                indexEnd = lyricsHtml.IndexOf("<", indexStart);
                var artistName = lyricsHtml.Substring(indexStart + 1, indexEnd - indexStart - 1);

                indexStart = lyricsHtml.IndexOf("<p class=\"songtext\" lang=\"EN\">") + "<p class=\"songtext\" lang=\"EN\">".Length;

                indexEnd = lyricsHtml.IndexOf("</p>");

                var lyrics = lyricsHtml.Substring(indexStart, indexEnd - indexStart);

                while (lyrics.IndexOf("[") > -1)
                {
                    lyrics = lyrics.Substring(0, lyrics.IndexOf("[")) + lyrics.Substring(lyrics.IndexOf("]") + 1);
                }

                var split = lyrics.Replace("\n", "").Replace("\r", "").Split(new[] { "<br />" }, StringSplitOptions.RemoveEmptyEntries);
                song = new SongResultModel()
                {
                    ArtistName = artistName,
                    Lyrics = split,
                    SongName = songName,
                };
            }
            catch (Exception ex)
            {

            }
            return song;
        }


        public SongResultModel GetSongFromLyricsDotCom(string search)
        {
            SongResultModel song = null;
            try
            {
                var lyricsUrl = $"http://www.lyrics.com/search.php?keyword={WebUtility.UrlEncode(search)}&what=all";
                var httpClient = new HttpClient();
                var lyricsTask = httpClient.GetStringAsync(lyricsUrl);
                lyricsTask.Wait();
                var lyricsHtml = lyricsTask.Result;
                var indexStart = lyricsHtml.IndexOf("class=\"lyrics_preview\"");
                int indexEnd;
                // didnt find an exact match, grab the first item in the list of matches
                //if (indexStart == -1)
                //{
                    indexStart = HtmlHelper.PreviousIndex(lyricsHtml, "<a", indexStart);
                    indexStart = lyricsHtml.IndexOf("href=\"", indexStart) + "href=\"".Length;
                    indexEnd = lyricsHtml.IndexOf("\"", indexStart);
                    var songUrl = "http://www.lyrics.com/" + lyricsHtml.Substring(indexStart, indexEnd - indexStart);
                    lyricsTask = httpClient.GetStringAsync(songUrl);
                    lyricsTask.Wait();
                    lyricsHtml = lyricsTask.Result;
                //}

                indexStart = lyricsHtml.IndexOf("id=\"profile_name\"");
                indexStart = lyricsHtml.IndexOf(">", indexStart) + ">".Length;
                indexEnd = lyricsHtml.IndexOf("<", indexStart);
                var songName = lyricsHtml.Substring(indexStart, indexEnd - indexStart).Trim();
                
                indexStart = lyricsHtml.IndexOf("<a", indexStart);
                indexStart = lyricsHtml.IndexOf(">", indexStart) + ">".Length;
                indexEnd = lyricsHtml.IndexOf("<", indexStart);
                var artistName = lyricsHtml.Substring(indexStart, indexEnd - indexStart);

                indexStart = lyricsHtml.IndexOf("id=\"lyrics\"") + "id=\"lyrics\"".Length;
                indexStart = lyricsHtml.IndexOf(">", indexStart) + ">".Length;
                indexEnd = lyricsHtml.IndexOf("</div>", indexStart);

                var lyrics = lyricsHtml.Substring(indexStart, indexEnd - indexStart);

                while (lyrics.IndexOf("[") > -1)
                {
                    lyrics = lyrics.Substring(0, lyrics.IndexOf("[")) + lyrics.Substring(lyrics.IndexOf("]") + 1);
                }

                var split = lyrics.Replace("\n", "").Replace("\r", "").Split(new[] { "<br />" }, StringSplitOptions.RemoveEmptyEntries);
                song = new SongResultModel()
                {
                    ArtistName = artistName,
                    Lyrics = split,
                    SongName = songName,
                };
            }
            catch (Exception ex)
            {

            }
            return song;
        }

        private Task SendVoice(ISendWrapper sender, string[] messages)
        {
            return Task.Run(() =>
            {
                try {
                    foreach (var message in messages)
                    {
                        if (!VoiceModule.IsStop)
                        {
                            var httpClient = new HttpClient();
                            var parameters = new Dictionary<string, string>();
                            var voiceSettings = VoiceHelpers.GetVoiceSettings();
                            parameters["MyLanguages"] = "sonid10";
                            parameters["MySelectedVoice"] = voiceSettings.CurrentVoice;
                            parameters["MyTextForTTS"] = message;
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
                                        if (vc.Connected && !VoiceModule.IsStop)
                                        {
                                            vc.SendVoice(buffer);
                                        }
                                        else
                                            break;
                                    }

                                    resampler.Dispose();
                                    mp3Reader.Close();

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }
    }
}
