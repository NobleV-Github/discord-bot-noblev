using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;

namespace discord_bot_noblev
{
    public static class Events
    {
        public static async Task MessageReceived(SocketMessage message)
        {
            //If the Author is a bot it leaves method
            if (message.Author.IsBot) { return; }
            
            //If the Message starts with the set Prefix
            if (message.Content.StartsWith("!"))
            {
                switch (message.Content.Remove(0, 1).Split(' ')[0])
                {
                    case "help":
                        await Log(new LogMessage(LogSeverity.Info, "help", $"Help by {message.Author.Mention}"));

                        var embed = new EmbedBuilder()
                        {
                            Title = "Information",
                            Description =
                                "Server\n➥ IP-Adresse: 45.138.50.38\n➥ Webseite: https://www.atomic-roleplay.eu/n➥ Teamspeak³: ts.atomic-rp.de\n\nNützliches\n➥ Server Status in #「:crystal_ball:」status\n➥ Alle Tastenkürzel in #「:keyboard:」tastenbelegung1\n➥ Hilfe zu Saltychat in #「:satellite:」saltychat-installieren\n➥ Alles zu Spenden in #「:credit_card:」spenden\n➥ Alle Regelwerkänderungen in #「:pushpin:」regelwerk-änderungen\n",
                            Color = Color.Gold
                        };

                        await message.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                }
            }
        }

        //Checks if User is admin on guild
        private static bool IsAuthorAdmin(SocketMessage message) => ((SocketGuildUser)message.Author).GuildPermissions.Administrator;

        //This method is used to write log
        public static Task Log(LogMessage msg)
        {
            Console.Write($"{DateTime.Now.ToString(CultureInfo.CurrentCulture).Remove(0, 11)} {msg.Severity} {msg.Source} {msg.Exception}");
            Console.SetCursorPosition(40, Console.CursorTop);
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }

        public static Task UserJoined(SocketGuildUser arg)
        {
            arg.AddRoleAsync(ulong.Parse(JObject.Parse(Program.Json)["auto_role"].ToString()));
            return Task.CompletedTask;
        }
    }
}
