using DSharpPlus;
using DSharpPlus.CommandsNext;
using McStatBot.Commands;
using McStatBot.Core;
using McStatBot.Core.Impl;
using System;
using System.Threading.Tasks;

namespace McStatBot
{
    class Program
    {
        public static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();

        private IGuildsMonitor guildsMonitor;
        private IGuildCollection guildsCollection;
        private IBotStore botStore;

        private async Task MainAsync(string[] args)
        {
            var token = Environment.GetEnvironmentVariable("TOKEN");
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Missing token");
                Environment.Exit(-1);
            }

            SetupBotEnvironment();
            ScheduleGuildMonitor();

            var bot = CreateBot(token);

            ConfigureBot(bot);

            await bot.ConnectAsync();
            await Task.Delay(-1);
        }

        private void ConfigureBot(DiscordClient bot)
        {
            bot.MessageCreated += TraceGuild;

            var commands = bot.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }
            });

            commands.RegisterCommands<MinecraftServerModule>();
            commands.RegisterCommands<PlayerModule>();
        }

        private DiscordClient CreateBot(string token)
        {
            return new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
            });
        }

        private void SetupBotEnvironment()
        {
            botStore = new BotStore();
            guildsMonitor = new GuildsMonitor(botStore);
            guildsCollection = guildsMonitor.Load();
        }

        private void ScheduleGuildMonitor()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    guildsMonitor.Persist();
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            });
        }

        private Task TraceGuild(DiscordClient sender, DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            guildsCollection.Trace(e.Guild.Name);

            return Task.CompletedTask;
        }
    }
}
