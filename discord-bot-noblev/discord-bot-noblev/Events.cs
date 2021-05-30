using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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

            bool mute = false;
          

            //If the Message starts with the set Prefix
            if (message.Content.StartsWith(JObject.Parse(Program.Json)["prefix"].ToString()))
            {
                switch (message.Content.Remove(0, 1).Split(' ')[0])
                {
                    case "help":
                        await Log(new LogMessage(LogSeverity.Info, "help", $"Help by {message.Author.Mention}"));

                        var help = new EmbedBuilder()
                        {
                            Title = "Information",
                            Description =
                                @"Server
                                ➥ Fivem: fivem.noblev.de
                                ➥ Webseite: https://www.noblev.de
                                ➥ Teamspeak³: ts.noblev.de

                                Nützliches
                                ➥ Server Status in <#842376024650285109>
                                ➥ Alle Tastenkürzel in #「:keyboard:」tastenbelegung
                                ➥ Alles zu Spenden in <#842375727029944330>
                                ➥ Alle Regelwerkänderungen in <#842374922017964042>
                                ",
                            Color = Color.Gold
                        };

                        await message.Channel.SendMessageAsync("", false, help.Build());
                        break;
                    case "command":
                        var commands = new EmbedBuilder()
                        {
                            Title = "Commands",
                            Description =
                                @"!help     = Get serverinfo
                                  !mute     = Mute user
                                  !unmute   = Unmute user
                                  !command  = This message
                                ",
                            Color = Color.Gold
                        };

                        await message.Channel.SendMessageAsync("", false, commands.Build());
                        break;
                    case "mute":
                        mute = true;
                        goto case "unmute";
                    case "unmute":
                        var guild = ((SocketGuildChannel)message.Channel).Guild;
                        SocketGuildUser user;
                        try
                        {
                            user = guild.GetUser(message.MentionedUsers.First().Id);
                        }
                        catch (Exception)
                        {
                            goto case "command";
                        }
                        var config = JObject.Parse(Program.Json)["mute_role"].ToString();
                        if (mute)
                        {
                            await user.AddRoleAsync(ulong.Parse(config));
                            await message.Channel.SendMessageAsync($"{user.Username} has been muted");
                        }
                        else
                        {
                            await user.RemoveRoleAsync(ulong.Parse(config));
                            await message.Channel.SendMessageAsync($"{user.Username} has been unmuted");
                        }
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
