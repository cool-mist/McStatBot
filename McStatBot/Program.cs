using DSharpPlus;
using DSharpPlus.CommandsNext;
using McStatBot.Commands;
using McStatBot.Container;
using McStatBot.Core.Config;
using McStatBot.Core.Guild;
using McStatBot.Utils;
using System;
using System.Threading.Tasks;

namespace McStatBot
{
    class Program
    {
        public static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();

        private async Task MainAsync(string[] args)
        {
            IServiceProvider serviceProvider = BotContainerFactory.Build().Initialize();
            DiscordClient bot = CreateBot(serviceProvider);

            ScheduleGuildMonitor(serviceProvider);

            await bot.ConnectAsync();
            await Task.Delay(-1);
        }

        private DiscordClient CreateBot(IServiceProvider serviceProvider)
        {
            string token = ReadToken(serviceProvider);

            DiscordClient bot = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
            });

            var commands = bot.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }, //TODO: Config
                Services = serviceProvider
            });

            commands.RegisterCommands<MinecraftServerModule>();
            commands.RegisterCommands<PlayerModule>();
            //commands.RegisterCommands<CoordinatesModule>();

            //commands.CommandErrored += (s, e) => e.Context.RespondAsync(string.Format(ErrorCodes.UnknownCommand, e.Context.Prefix));
            bot.MessageCreated += (sender, e) => serviceProvider.Resolve<IGuildCollection>().Trace(e.Guild.Name);

            return bot;
        }

        private string ReadToken(IServiceProvider serviceProvider)
        {
            var token = serviceProvider.Resolve<IConfig>().Get("TOKEN"); //TODO: Config
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Missing token");
                Environment.Exit(-1);
            }

            return token;
        }

        private void ScheduleGuildMonitor(IServiceProvider serviceProvider)
        {
            IGuildsMonitor guildsMonitor = serviceProvider.Resolve<IGuildsMonitor>();

            Task.Run(async () =>
            {
                while (true)
                {
                    guildsMonitor.Persist();
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            });
        }
    }
}
