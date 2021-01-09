using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using McStatBot.Core;
using McStatBot.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace McStatBot.Commands
{
    public class MinecraftServerModule : BaseCommandModule
    {
        private readonly Dictionary<string, string> serverNames = new Dictionary<string, string>();
        private readonly IMinecraftServerClient minecraftServerStatsClient = new McSrvrStatClient();

        [Command("status")]
        [Description("Shows the status of a minecraft server")]
        public async Task StatusCommand(CommandContext ctx, [Description("Name or IP of the server")] string serverName)
        {
            await DoShow(ctx, serverName);
        }

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

        private async Task DoShow(CommandContext ctx, string serverName)
        {
            try
            {
                var minecraftServer = await minecraftServerStatsClient.GetServerStatus(serverName);

                if (minecraftServer == null)
                {
                    await ctx.RespondAsync(ErrorCodes.GenericExceptionMessage);
                    return;
                }

                if (!minecraftServer.Online)
                {
                    await ctx.RespondAsync($"{serverName} is not online");
                    return;
                }

                var discordEmbed = new DiscordEmbedBuilder()
                    .WithAuthor($"Status for {minecraftServer.Hostname}", null, minecraftServer.Icon)
                    .WithColor(DiscordColor.DarkRed)
                    .WithThumbnail(minecraftServer.Icon)
                    .AddField("Players", $"{minecraftServer.OnlinePlayers}/{minecraftServer.MaxPlayers}", inline: true)
                    .AddField("Version", $"{minecraftServer.Version}", inline: true)
                    .AddField("Motd", $"{string.Join('\n', minecraftServer.Motd)}")
                    .Build();

                await ctx.RespondAsync(null, false, discordEmbed);

                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await ctx.RespondAsync(ErrorCodes.GenericExceptionMessage);
            }
        }
    }
}
