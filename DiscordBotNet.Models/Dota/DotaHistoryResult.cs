using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBotNet.Models.Dota
{
    [JsonObject]
    public class DotaHistoryResult
    {
        public DotaHistory Result { get; set; }
    }

    [JsonObject]
    public class DotaHistory
    {
        public IEnumerable<DotaMatch> Matches { get; set; }

        [JsonProperty("num_results")]
        public int NumResults { get; set; }

        [JsonProperty("results_remaining")]
        public int ResultsRemaining { get; set; }

        public int Status { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }

    [JsonObject]
    public class DotaMatch
    {
        [JsonProperty("dire_team_id")]
        public int DireTeamId { get; set; }
        [JsonProperty("lobby_type")]
        public int LobbyType { get; set; }

        [JsonProperty("match_id")]
        public long MatchId { get; set; }

        [JsonProperty("match_seq_num")]
        public long MatchSeqNum { get; set; }

        public IEnumerable<DotaPlayer> Players { get; set; }

        [JsonProperty("radiant_team_id")]
        public int RadiantTeamId { get; set; }

        [JsonProperty("start_time")]
        public long StartTime { get; set; }
    }

    [JsonObject]
    public class DotaPlayer
    {
        [JsonProperty("account_id")]
        public long AccountId { get; set; }
        [JsonProperty("hero_id")]
        public long HeroId { get; set; }
        [JsonProperty("player_slot")]
        public long PlayerSlot { get; set; }
    }
}
