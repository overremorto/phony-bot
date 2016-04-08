using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBotNet.Models.Dota
{
    [JsonObject]
    public class DotaMatchResult
    {
        public DotaMatchFull Result { get; set; }
    }

    [JsonObject]
    public class DotaMatchFull
    {
        [JsonProperty("barracks_status_dire")]
        public int BarrackStatusDire { get; set; }

        [JsonProperty("barracks_status_radiant")]
        public int BarrackStatusRadiant { get; set; }

        [JsonProperty("cluster")]
        public int Cluster { get; set; }

        [JsonProperty("dire_score")]
        public int DireScore { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("engine")]
        public int Engine { get; set; }

        [JsonProperty("first_blood_time")]
        public int FirstBloodTime { get; set; }

        [JsonProperty("flags")]
        public int Flags { get; set; }

        [JsonProperty("game_mode")]
        public int GameMode { get; set; }

        [JsonProperty("human_players")]
        public int HumanPlayers { get; set; }

        [JsonProperty("leagueid")]
        public int LeagueId { get; set; }

        [JsonProperty("lobby_type")]
        public int LobbyType { get; set; }

        [JsonProperty("match_id")]
        public long MatchId { get; set; }

        [JsonProperty("match_seq_num")]
        public long MatchSeqNum { get; set; }

        [JsonProperty("negative_votes")]
        public int NegativeVotes { get; set; }

        [JsonProperty("positive_votes")]
        public int PositiveVotes { get; set; }

        [JsonProperty("radiant_score")]
        public int RadiantScore { get; set; }

        [JsonProperty("radiant_win")]
        public bool RadiantWin { get; set; }

        [JsonProperty("start_time")]
        public long StartTime { get; set; }

        [JsonProperty("tower_status_dire")]
        public int TowerStatusDire { get; set; }

        [JsonProperty("tower_status_radiant")]
        public int TowerStatusRadiant { get; set; }

        [JsonProperty("players")]
        public IEnumerable<PlayerMatch> Players { get; set; }
    }

    [JsonObject]
    public class PlayerMatch
    {
        [JsonProperty("account_id")]
        public long AccountId { get; set; }

        [JsonProperty("assists")]
        public int Assists { get; set; }

        [JsonProperty("deaths")]
        public int Deaths { get; set; }

        [JsonProperty("denies")]
        public int Denies { get; set; }

        [JsonProperty("gold")]
        public int Gold { get; set; }

        [JsonProperty("gold_per_min")]
        public int GoldPerMin { get; set; }

        [JsonProperty("gold_spent")]
        public int GoldSpent { get; set; }

        [JsonProperty("hero_damage")]
        public int HeroDamage { get; set; }

        [JsonProperty("hero_healing")]
        public int HeroHealing { get; set; }

        [JsonProperty("hero_id")]
        public int HeroId { get; set; }

        [JsonProperty("item_0")]
        public int Item0 { get; set; }

        [JsonProperty("item_1")]
        public int Item1 { get; set; }

        [JsonProperty("item_2")]
        public int Item2 { get; set; }

        [JsonProperty("item_3")]
        public int Item3 { get; set; }

        [JsonProperty("item_4")]
        public int Item4 { get; set; }

        [JsonProperty("item_5")]
        public int Item5 { get; set; }

        [JsonProperty("kills")]
        public int Kills { get; set; }

        [JsonProperty("last_hits")]
        public int LastHits { get; set; }

        [JsonProperty("leaver_status")]
        public int LeaverStatus { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("player_slot")]
        public int PlayerSlot { get; set; }

        [JsonProperty("tower_damage")]
        public int TowerDamage { get; set; }

        [JsonProperty("xp_per_min")]
        public int XpPerMin { get; set; }
    }
}
