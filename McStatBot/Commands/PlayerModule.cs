using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using McStatBot.Core.PlayerProfile;
using McStatBot.Core.PlayerProfile.Impl;
using McStatBot.Utils;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McStatBot.Commands
{
    public class PlayerModule : BaseCommandModule
    {
        private readonly IMojangClient mojangClient = new MojangClient();

        [Command("profile")]
        [Description("Shows the profile of a player")]
        public async Task ProfileCommand(CommandContext ctx, [Description("Name of the player")] string playerName)
        {
            var player = await mojangClient.GetPlayer(playerName);

            if (player == null)
            {
                await ctx.RespondAsync(ErrorCodes.GenericExceptionMessage);
                return;
            }

            if (player.Found == false)
            {
                await ctx.RespondAsync(string.Format(ErrorCodes.PlayerNotFound, playerName));
                return;
            }

            var playerNamesHistoryBuilder = new StringBuilder();

            var sortedPlayerNames = player.Names.ToList()
                .OrderByDescending(n => n.Since);

            int nameCount = sortedPlayerNames.Count();

            foreach (var name in sortedPlayerNames)
            {
                playerNamesHistoryBuilder.Append($"**{nameCount--}**. `{name.Name}` | {GetDateString(name.Since)}\n");
            }

            var discordEmbed = new DiscordEmbedBuilder()
                .WithAuthor($"Profile for {player.Name}", null)
                .WithDescription($"Short UUID: `{player.Id:N}`\nLong UUID : `{player.Id:D}`")
                .WithColor(DiscordColor.Blue)
                .WithThumbnail(player.Skin)
                .AddField("Skin", $"[View Image]({player.Skin})", inline: true)
                .AddField("Type", $"`{player.SkinType}`", true)
                .AddField("Account", $"Legacy: `{player.Legacy}`\nDemo: `{player.Demo}`", inline: true)
                .AddField("Name History", playerNamesHistoryBuilder.ToString())
                .Build();

            await ctx.RespondAsync(null, false, discordEmbed);
        }

        private string GetDateString(DateTime since)
        {
            if (since.IsEpoch())
            {
                return "Initial";
            }

            return since.ToString("dd MMMM yyyy");
        }
    }
}
