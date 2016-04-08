using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.FileHelpers;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Models.Dota;
using Newtonsoft.Json;

namespace DiscordBotNet.Module.Command
{
    public class DotaLastMatchCommand : BaseCommand
    {
        public DotaLastMatchCommand() : base("last-match", "Most Recent Dota Match", "Displays your most recent Dota match with who won and a link to the dotabuff")
        {
        }

        public override bool Execute(ISendWrapper sender)
        {
            if (!base.Execute(sender))
            {
                return false;
            }

            var players = DotaFileHelpers.GetDotaPlayers();
            var dotaSettings = DotaFileHelpers.GetDotaSettings();

            var callerSteamId = players.Any(p => p.Value.Name == sender.Author.Username) ? players.FirstOrDefault(p => p.Value.Name == sender.Author.Username).Key : null;

            if (!string.IsNullOrEmpty(callerSteamId))
            {
                var url = $"https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key={dotaSettings.ApiKey}&account_id={callerSteamId}&matches_requested=1";

                HttpClient httpClient = new HttpClient();
                var task = httpClient.GetStringAsync(url);
                task.Wait();
                var dotaResponse = JsonConvert.DeserializeObject<DotaHistoryResult>(task.Result);


                var isRadiant = dotaResponse.Result.Matches.FirstOrDefault().Players?.FirstOrDefault(p => p.AccountId.ToString() == callerSteamId)?.PlayerSlot <= 4;

                var matchId = dotaResponse.Result.Matches.FirstOrDefault().MatchId;
                var url2 = $"https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?key={dotaSettings.ApiKey}&match_id={matchId}";

                HttpClient httpClient2 = new HttpClient();
                var task2 = httpClient2.GetStringAsync(url2);
                task2.Wait();
                var matchResponse = JsonConvert.DeserializeObject<DotaMatchResult>(task2.Result);

                var didRadiantWin = matchResponse.Result.RadiantWin;
                var didIWin = (isRadiant && didRadiantWin) || (!isRadiant && !didRadiantWin);

                if (didIWin)
                {
                    sender.SendMessage($"The last game you played was a WIN! Yay!", Module.Manager.DiscordSettings.IsTTS);
                }
                else
                {
                    sender.SendMessage($"Unfortunately the last game you played was a LOSS. I'm sorry. I'm so sorry. :(", Module.Manager.DiscordSettings.IsTTS);
                }

                sender.SendMessage($"Check out the match details - http://www.dotabuff.com/matches/{matchId}", Module.Manager.DiscordSettings.IsTTS);
            }
            else
            {
                sender.SendMessage($"User {sender.Author.Username} not found!", Module.Manager.DiscordSettings.IsTTS);
            }

            return true;
        }
    }
}
