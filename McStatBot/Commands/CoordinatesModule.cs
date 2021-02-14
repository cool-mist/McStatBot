using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace McStatBot.Commands
{
    [Group("coords")]
    public class CoordinatesModule : BaseCommandModule
    {

        [Command("show")]
        [Description("Shows all the stored coordinates")]
        public async Task ShowCommand(CommandContext ctx)
        {
            await ctx.RespondAsync("Not yet implemented!");
        }

        [Command("save")]
        [Description("Save a command")]
        public async Task SaveCommand(CommandContext ctx,
            [Description("Dimension - o|n|e")] string dimension,
            [Description("x coordinate")] string x,
            [Description("y coordinate")] string y,
            [Description("z coordinate")] string z,
            [Description("Name")] string name)
        {
            await ctx.RespondAsync("Not yet implemented!");
        }
    }
}
