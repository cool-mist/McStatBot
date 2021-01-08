using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using McStatBot.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace McStatBot.Commands
{
    public class MinecraftStatModule : BaseCommandModule
    {
        private readonly Dictionary<string, string> serverNames = new Dictionary<string, string>();
        private readonly IMinecraftServerStatsClient minecraftServerStatsClient = new McSrvrStat();

        [Command("register")]
        [Description("Registers and shows the status of a minecraft server name to view statuses for")]
        public async Task RegisterCommand(CommandContext ctx, [Description("Name or IP of the server")] string serverName)
        {

            if (this.serverNames.ContainsKey(ctx.Guild.Name))
            {
                this.serverNames[ctx.Guild.Name] = serverName;
            }
            else
            {
                this.serverNames.Add(ctx.Guild.Name, serverName);
            }

            await ctx.RespondAsync($"Registered {serverName}");
            await ShowCommand(ctx);
        }

        [Command("show")]
        [Description("Shows the status of the currently regisered minecraft server")]
        public async Task ShowCommand(CommandContext ctx)
        {
            if (!this.serverNames.TryGetValue(ctx.Guild.Name, out var serverName))
            {
                await ctx.RespondAsync("Register a server first");
                return;
            }

            await DoShow(ctx, serverName);
        }

        [Command("status")]
        [Description("Shows the status of a minecraft server")]
        public async Task StatusCommand(CommandContext ctx, [Description("Name or IP of the server")] string serverName)
        {
            await DoShow(ctx, serverName);
        }

        private async Task DoShow(CommandContext ctx, string serverName)
        {
            try
            {
                var status = await minecraftServerStatsClient.GetStatus(serverName);

                if (status == null)
                {
                    await ctx.RespondAsync($"Something went wrong, please try again.");
                    return;
                }

                if (!status.Online)
                {
                    await ctx.RespondAsync($"{serverName} is not online");
                    return;
                }

                var serverIcon = status.Icon;

                var discordEmbed = new DiscordEmbedBuilder()
                    .WithAuthor(status.Hostname, null, serverIcon)
                    .WithColor(DiscordColor.DarkRed)
                    .WithThumbnail(serverIcon)
                    .AddField("Players", $"{status.Players.Online}/{status.Players.Max}", inline: true)
                    .AddField("Version", $"{status.Version}", inline: true)
                    .AddField("Motd", $"{string.Join('\n', status.Motd.Clean)}")
                    .Build();

                await ctx.RespondAsync(null, false, discordEmbed);

                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await ctx.RespondAsync($"Something went wrong, please try again.");
            }
        }
    }
}
