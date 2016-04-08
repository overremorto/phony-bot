using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiscordBotNet.FileHelpers;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Models.Dota;
using DiscordSharp.Objects;
using Newtonsoft.Json;

namespace DiscordBotNet.Module.Command
{
    public class DotaScanCommand : BaseCommand
    {
        private bool _isRunning;
        private DiscordChannel _discordChannel;
        private int _pollTimer;
        private System.Timers.Timer _timer;
        ISendWrapper _sender;
        public DotaScanCommand() : base("scan", "Scan", "Reports you last match when you finish it.")
        {
            _pollTimer = 60000;
            _isRunning = false;
            _timer = new System.Timers.Timer();
            _timer.Interval = _pollTimer;
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
        }


        public override bool Execute(ISendWrapper sender)
        {
            if (!base.Execute(sender))
            {
                return false;
            }

            if (!_isRunning)
            {
                _isRunning = true;
                _sender = sender;
                _timer.Start();
                sender.SendMessage("Starting Dota scan...");
            }
            else
            {
                _isRunning = false;
                _sender = sender;
                _timer.Stop();
                sender.SendMessage("Stopping Dota scan...");
            }

            return true;
        }


        private void _timer_Elapsed(object sender1, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var dotaSettings = DotaFileHelpers.GetDotaSettings();
                // check steam api for me (maybe other people too...) if changes from last pull (1 min)
                // if changes report who won, and other things...
                var url = $"https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key={dotaSettings.ApiKey}&account_id={dotaSettings.MySteamAccountId}&matches_requested=1";

                HttpClient client = new HttpClient();
                var task = client.GetStringAsync(url);
                task.Wait();
                var dotaResponse = JsonConvert.DeserializeObject<DotaHistoryResult>(task.Result);

                var storedSettings = DotaFileHelpers.GetDotaStoredSettings();

                // if changed
                // store last match
                if (storedSettings.LastMatchId != dotaResponse?.Result?.Matches?.FirstOrDefault()?.MatchId)
                {
                    storedSettings.LastMatchId = (dotaResponse?.Result?.Matches?.FirstOrDefault()?.MatchId).Value;
                    storedSettings.SentMessageForLastMatch = false;
                    var isRadiant = dotaResponse.Result.Matches.FirstOrDefault().Players?.FirstOrDefault(p => p.AccountId.ToString() == dotaSettings.MySteamAccountId)?.PlayerSlot <= 4;

                    var matchId = dotaResponse.Result.Matches.FirstOrDefault().MatchId;
                    var url2 = $"https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?key={dotaSettings.ApiKey}&match_id={matchId}";

                    HttpClient client2 = new HttpClient();
                    var task2 = client2.GetStringAsync(url2);
                    task2.Wait();
                    var matchResponse = JsonConvert.DeserializeObject<DotaMatchResult>(task2.Result);

                    var didRadiantWin = matchResponse.Result.RadiantWin;
                    var didIWin = (isRadiant && didRadiantWin) || (!isRadiant && !didRadiantWin);
                    storedSettings.DidWinLastMatch = didIWin;

                    var players = DotaFileHelpers.GetDotaPlayers();

                    var myPlayers = matchResponse.Result.Players.Where(p => players.Keys.Contains(p.AccountId.ToString())).ToList();
                    var highestKills = myPlayers.OrderByDescending(p => p.Kills).FirstOrDefault();
                    var highestGpm = myPlayers.OrderByDescending(p => p.GoldPerMin).FirstOrDefault();
                    var highestKda = myPlayers.OrderByDescending(p => p.Deaths > 0 ? (p.Kills + p.Assists) / (double)p.Deaths : double.MaxValue).FirstOrDefault();
                    storedSettings.LastMatchHighestKills = $"{players[highestKills.AccountId.ToString()].Name} - {highestKills.Kills} Kills";
                    storedSettings.LastMatchHighestGPM = $"{players[highestGpm.AccountId.ToString()].Name} - {highestGpm.GoldPerMin} GPM";
                    storedSettings.LastMatchHighestKDA = $"{players[highestKda.AccountId.ToString()].Name} - {(highestKda.Deaths > 0 ? ((highestKda.Kills + highestKda.Assists) / (double)highestKda.Deaths).ToString("n2") : (highestKda.Kills + highestKda.Assists).ToString("n2"))}({highestKda.Kills + highestKda.Assists}/{highestKda.Deaths}) KDA{(highestKda.Deaths == 0 ? " (No deaths, way to go!)" : "")}";


                    DotaFileHelpers.SetDotaStoredSettings(storedSettings);
                }

                storedSettings = DotaFileHelpers.GetDotaStoredSettings();

                if (_sender != null && !storedSettings.SentMessageForLastMatch)
                {

                    if (storedSettings.DidWinLastMatch)
                    {
                        _sender.SendMessage($"We WON the last DOTA game. Good work everyone! Shoutout to the these guys!");

                        _sender.SendMessage($"Most Kills: {storedSettings.LastMatchHighestKills}");
                        _sender.SendMessage($"Highest GPM: {storedSettings.LastMatchHighestGPM}");
                        _sender.SendMessage($"Highest KDA: {storedSettings.LastMatchHighestKDA}");

                        _sender.SendMessage($"Check out the match details - http://www.dotabuff.com/matches/{storedSettings.LastMatchId}");
                    }
                    else
                    {
                        _sender.SendMessage($"Unfortunately we LOST the last DOTA game. Don't worry guys we'll win the next one... (unless we continue to play like last game). At least these players did less worse than the rest...");

                        _sender.SendMessage($"Most Kills: {storedSettings.LastMatchHighestKills}");
                        _sender.SendMessage($"Highest GPM: {storedSettings.LastMatchHighestGPM}");
                        _sender.SendMessage($"Highest KDA: {storedSettings.LastMatchHighestKDA}");

                        _sender.SendMessage($"Check out the match details - http://www.dotabuff.com/matches/{storedSettings.LastMatchId}");
                    }

                    storedSettings.SentMessageForLastMatch = true;
                    DotaFileHelpers.SetDotaStoredSettings(storedSettings);
                }
            }
            catch
            {

            }
        }

    }
}
