using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.Game;

namespace DiscordBotNet.FileHelpers
{
    public static class WebAdventureHelpers
    {
        public static IEnumerable<GameModel> GameList()
        {
            var httpClient = new HttpClient();
            var responseTask = httpClient.GetAsync("http://www.web-adventures.org/games.html");
            responseTask.Wait();
            var responseStringTask = responseTask.Result.Content.ReadAsStringAsync();
            responseStringTask.Wait();
            var result = responseStringTask.Result;
            var idx = HtmlHelper.GetElementIndex(result, "id", "games");
            var counter = 0;
            var games = new List<GameModel>();
            result = result.Substring(idx, result.IndexOf("</table>") - idx).Replace("<br>", "");
            idx = 0;
            while (result.IndexOf("<tr", idx) > -1)
            {
                idx = result.IndexOf("<tr", idx);

                // the first 2 rows are the headers, skip those
                if (counter > 1)
                {
                    var url = HtmlHelper.XmlSearch(result, "href", idx - 1);
                    var end = result.IndexOf("</a>", idx);
                    idx = result.LastIndexOf(">", end) + ">".Length;
                    var gameTitle = result.Substring(idx, end - idx);

                    idx = result.IndexOf("<td>", idx) + "<td>".Length;
                    end = result.IndexOf("</td>", idx);

                    var description = result.Substring(idx, end - idx);

                    var idIdx = url.LastIndexOf("s=") + "s=".Length;
                    var id = url.Substring(idIdx);
                    var game = new GameModel()
                    {
                        Title = gameTitle,
                        Description = description,
                        Id = id,
                        Url = url,
                    };

                    games.Add(game);

                }

                counter++;
            }

            return games;
        }


        public static GameResponse SendMessageToGame(ActiveGameModel game, string message)
        {

            var httpClient = new HttpClient();
            var values = new Dictionary<string, string>();
            values["s"] = game.Game.Id;
            values["x"] = game.SessionId;
            values["a"] = message ?? string.Empty;
            var content = new FormUrlEncodedContent(values);
            var responseTask = httpClient.PostAsync(game.Game.Url, content);
            responseTask.Wait();
            var responseStringTask = responseTask.Result.Content.ReadAsStringAsync();
            responseStringTask.Wait();
            var result = responseStringTask.Result;

            var start = HtmlHelper.GetLastElementIndex(result, "class", "status");
            var end = result.IndexOf("</td>", start);

            var text = result.Substring(start, end - start)
                .Replace("<b>", "")
                .Replace("</b>", "")
                .Replace("\n", "")
                .Replace("<p class=\"status\">", "")
                .Replace("</p>", "")
                .Split(new[] { "<br><br>" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Replace("<br>", ""));

            //var id = HtmlHelper.XmlSearch(result, "value", HtmlHelper.GetElementIndex(result, "id", "x"));

            var resp = new GameResponse()
            {
                Text = text,
                SessionId = null,
            };

            return resp;
        }

        public static GameResponse LoadGame(GameModel game, string sessionId)
        {
            var httpClient = new HttpClient();
            var responseTask = httpClient.GetAsync(game.Url + (!string.IsNullOrEmpty(sessionId) ? $"&x={sessionId}" : string.Empty));
            responseTask.Wait();
            var responseStringTask = responseTask.Result.Content.ReadAsStringAsync();
            responseStringTask.Wait();
            var result = responseStringTask.Result;

            var start = HtmlHelper.GetElementIndex(result, "class", "status");
            var end = result.IndexOf("</td>", start);

            var text = result.Substring(start, end - start)
                .Replace("<b>", "")
                .Replace("</b>", "")
                .Replace("\n", "")
                .Replace("<p class=\"status\">", "")
                .Replace("</p>", "")
                .Split(new[] { "<br><br>" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Replace("<br>", ""));

            var id = HtmlHelper.XmlSearch(result, "value", HtmlHelper.GetElementIndex(result, "id", "x"));

            var resp = new GameResponse()
            {
                Text = text,
                SessionId = id,
            };

            return resp;
        }

        public static GameResponse NewGame(GameModel game)
        {
            return LoadGame(game, null);
        }
    }
}
