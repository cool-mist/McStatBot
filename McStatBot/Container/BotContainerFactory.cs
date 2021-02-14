using McStatBot.Core;
using McStatBot.Core.Config;
using McStatBot.Core.Config.Impl;
using McStatBot.Core.Guild;
using McStatBot.Core.Guild.Impl;
using McStatBot.Core.Impl;
using McStatBot.Core.PlayerProfile;
using McStatBot.Core.PlayerProfile.Impl;
using McStatBot.Core.ServerStatus;
using McStatBot.Core.ServerStatus.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace McStatBot.Container
{
    public static class BotContainerFactory
    {
        public static IBotContainer Build()
        {
            return new BotContainer();
        }
    }

    internal class BotContainer : IBotContainer
    {
        public IServiceProvider Initialize()
        {
            IBotStore botStore = new BotStore();
            IGuildsMonitor guildsMonitor = new GuildsMonitor(botStore);
            IGuildCollection guildsCollection = guildsMonitor.Load();
            IMinecraftPlayerClient mojangClient = new MinecraftPlayerClient();
            IMinecraftServerClient minecraftServerClient = new McSrvrStatClient();
            IConfig config = new ConfigStore();

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton(botStore)
                .AddSingleton(guildsCollection)
                .AddSingleton(guildsMonitor)
                .AddSingleton(mojangClient)
                .AddSingleton(minecraftServerClient)
                .AddSingleton(config)
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
