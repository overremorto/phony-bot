using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiscordBotNet.FileHelpers;
using DiscordBotNet.Models.Calculator;
using DiscordBotNet.Models.ChatBot;
using DiscordBotNet.Models.DiscordWrappers;
using DiscordBotNet.Models.Dota;
using DiscordBotNet.Module.Module;
using DiscordBotNet.Module.ModuleManager;
using DiscordSharp;
using DiscordSharp.Objects;
using Newtonsoft.Json;


namespace Discord_bot_net
{
    public class Program
    {
        //private static string s_dotaApiKey = "8431BDA1561651D24886E034108C33FF";
        //private static int s_mySteamAccountId = 89224615;
        private static Dictionary<string, string> s_userName_chatBotUserId = new Dictionary<string, string>();
        private static DiscordChannel s_currentChannel = null;

        private static string s_apiKey = "LR2hvE9Zyq8i1MDq";
        //private static int s_chatBot = 23969;// wizard
        //private static int s_chatBot = 23958;// prob
        private static int s_chatBot = 2;// robot

        private static bool s_tts = false;
        private static DiscordSettings s_settings;

        private static IModuleManager s_moduleManager;

        public static void Main(string[] args)
        {
            s_settings = new DiscordSettings("!");
            s_moduleManager = new ModuleManager(s_settings);
            HttpClient httpClient = new HttpClient();
            var pollTimer = 60000;
            var loginSettings = DiscordHelpers.GetDiscordSettings();
            // First of all, a DiscordClient will be created, and the email and password will be defined.
            DiscordClient client = new DiscordClient();
            client.ClientPrivateInformation.Email = loginSettings.Email;
            client.ClientPrivateInformation.Password = loginSettings.Password;
            LoadModules();
            // Then, we are going to set up our events before connecting to discord, to make sure nothing goes wrong.

            client.Connected += (sender, e) => // Client is connected to Discord
            {
                Console.WriteLine("Connected! User: " + e.User.Username);
                // If the bot is connected, this message will show.
                // Changes to client, like playing game should be called when the client is connected,
                // just to make sure nothing goes wrong.
                client.UpdateCurrentGame("Bot online!"); // This will display at "Playing: "
            };


            client.PrivateMessageReceived += (sender, e) => // Private message has been received
            {

                MessageReceived(new PrivateSendWrapper(e.Author, e.Message, client));
            };


            client.MessageReceived += (sender, e) => // Channel message has been received
            {
                if (s_currentChannel == null)
                {
                    s_currentChannel = e.Channel;
                }

                if (e.Author.Username == s_settings.BotName)
                {
                    return;
                }
                MessageReceived(new ChannelSendWrapper(e.Channel, e.MessageText, e.Author, client));
            };

            
            
            // Now, try to connect to Discord.
            try
            {
                // Make sure that IF something goes wrong, the user will be notified.
                // The SendLoginRequest should be called after the events are defined, to prevent issues.
                client.SendLoginRequest();
                client.Connect(); // Login request, and then connect using the discordclient i just made.
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong!\n" + e.Message + "\nPress any key to close this window.");
            }

            //// Done! your very own Discord bot is online!

            Thread.Sleep(-1);
        }

        private static void GetDebugLogger_LogMessageReceived1(object sender, LoggerMessageReceivedArgs e)
        {
            throw new NotImplementedException();
        }

        private static void LoadModules()
        {
            s_moduleManager.InstallModule(new HelpModule());
            s_moduleManager.InstallModule(new PhonyModule());
            s_moduleManager.InstallModule(new PhonyTtsModule());
            s_moduleManager.InstallModule(new PhonyDotaModule());
            s_moduleManager.InstallModule(new PhonyMathModule());
            s_moduleManager.InstallModule(new PhonyPhoneModule());
            s_moduleManager.InstallModule(new PhonyVoiceModule());

            s_moduleManager.Init();
        }

        private static void MessageReceived(ISendWrapper sendWrapper)
        {
            try
            {
                s_moduleManager.HandleRequest(sendWrapper);
            }
            catch (Exception ex)
            {

            }
        }



        public int stringnumber(string name, int min, int max)
        {
            // Bonus code: returns number based of bytes of string.
            // If something goes wrong, (eg: too long int) it return the min value.
            // This is fun for commands to "rate" a user or anything else, and make sure the same string returns the same number
            try
            {
                byte[] namebt = Encoding.UTF8.GetBytes(name);
                string namebtstring = namebt.ToString();
                int namebtint = Int32.Parse(namebtstring);
                Random rnd = new Random(namebtint);
                int returnint = rnd.Next(min, max);
                return returnint;
            }
            catch (Exception)
            {
                return min;
            }
        }


        static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
                sbinary += buff[i].ToString("X2"); /* hex format */
            return sbinary;
        }
        
        private static string GenerateChatBotUserId()
        {
            var guid = Guid.NewGuid().ToString();
            guid = guid.Substring(0, guid.Length / 2);
            return guid;
        }



    }
}
