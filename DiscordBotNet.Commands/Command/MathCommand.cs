using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DiscordBotNet.Models.Calculator;
using DiscordBotNet.Models.DiscordWrappers;
using Newtonsoft.Json;

namespace DiscordBotNet.Module.Command
{
    public class MathCommand : BaseCommand
    {
        public MathCommand() : base("", "Math", "Can perform complex equations for all of you mathing needs. eg 2+2; solve(3x+2x=25);")
        {
        }

        public override bool Execute(ISendWrapper sender)
        {
            var httpClient = new HttpClient();
            var values = new Dictionary<string, string>();
            values["in[]"] = sender.RemainingMessage;
            values["trig"] = "deg";
            values["p"] = "0";
            values["s"] = "0";
            var content = new FormUrlEncodedContent(values);
            var responseTask = httpClient.PostAsync("http://web2.0calc.com/calc", content);
            responseTask.Wait();
            var responseStringTask = responseTask.Result.Content.ReadAsStringAsync();
            responseStringTask.Wait();

            var response = JsonConvert.DeserializeObject<WebCalcResponse>(responseStringTask.Result);
            sender.SendMessage(response.GetMessageText(), Module.Manager.DiscordSettings.IsTTS);
            return true;
        }
    }
}
