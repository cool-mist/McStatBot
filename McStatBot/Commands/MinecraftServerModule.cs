using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using McStatBot.Core.Guild;
using MinecraftUtils.Api;
using System;
using System.Threading.Tasks;

namespace McStatBot.Commands
{
    public class MinecraftServerModule : BaseCommandModule
    {
        private readonly IGuildCollection guilds;
        private readonly IMinecraftClient minecraftServerStatsClient;

        public MinecraftServerModule(IGuildCollection guilds, IMinecraftClient minecraftServerClient)
        {
            this.guilds = guilds;
            this.minecraftServerStatsClient = minecraftServerClient;
        }

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
            IGuildDetails guild = guilds.GetGuild(ctx.Guild.Name);
            guild.DefaultServerName = serverName;

            await ctx.RespondAsync($"Registered {serverName}");
            await ShowCommand(ctx);
        }

        [Command("show")]
        [Description("Shows the status of the currently regisered minecraft server")]
        public async Task ShowCommand(CommandContext ctx)
        {
            string serverName = guilds.GetGuild(ctx.Guild.Name).DefaultServerName;

            if (serverName == null)
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
                var minecraftServer = await minecraftServerStatsClient.GetStateAsync(serverName);

                if (minecraftServer == null)
                {
                    await ctx.RespondAsync(ErrorCodes.GenericExceptionMessage);
                    return;
                }

                IMinecraftState serverState = minecraftServer.Result;

                if (!(serverState?.State == "Online"))
                {
                    await ctx.RespondAsync($"{serverName} is not online");
                    return;
                }

                var discordEmbed = new DiscordEmbedBuilder()
                    .WithAuthor($"Status for {serverState.Hostname}", null, serverState.Icon)
                    .WithColor(DiscordColor.DarkRed)
                    .WithThumbnail($"https://api.mcsrvstat.us/icon/{serverName}")
                    .AddField("Players", $"{serverState.OnlinePlayers}/{serverState.MaxPlayers}", inline: true)
                    .AddField("Version", $"{serverState.Version}", inline: true)
                    .AddField("Motd", $"{string.Join('\n', serverState.Motd)}")
                    .Build();

                await ctx.RespondAsync(null, false, discordEmbed);

                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await ctx.RespondAsync(ErrorCodes.ServerIsNotUp);
            }
        }
    }
}
