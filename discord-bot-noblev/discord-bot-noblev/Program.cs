using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;

namespace discord_bot_noblev
{
    class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public static DiscordSocketClient Client;
        
        public static readonly string Json = File.ReadAllText(@"..\..\..\..\..\config.json");

        private async Task MainAsync()
        {
            //Client Events
            Client = new DiscordSocketClient();
            Client.MessageReceived += Events.MessageReceived;

            //Gets Token from Json
            var token = JObject.Parse(Json)["api"]["token"].ToString();

            //Logging in 
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
