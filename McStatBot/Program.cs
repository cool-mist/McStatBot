using DSharpPlus;
using DSharpPlus.CommandsNext;
using McStatBot.Commands;
using System;
using System.Threading.Tasks;

namespace McStatBot
{
    class Program
    {
        public static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();

        private async Task MainAsync(string[] args)
        {
            var token = Environment.GetEnvironmentVariable("TOKEN");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Missing token");
                Environment.Exit(-1);
            }

            var bot = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,

            });

            var commands = bot.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }
            });

            commands.RegisterCommands<MinecraftServerModule>();
            commands.RegisterCommands<PlayerModule>();

            await bot.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}
