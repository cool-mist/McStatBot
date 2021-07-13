using McStatBot.Core;
using McStatBot.Core.Config;
using McStatBot.Core.Config.Impl;
using McStatBot.Core.Guild;
using McStatBot.Core.Guild.Impl;
using McStatBot.Core.Impl;
using McStatBot.Core.PlayerProfile;
using McStatBot.Core.PlayerProfile.Impl;
using Microsoft.Extensions.DependencyInjection;
using MinecraftUtils.Api;
using MinecraftUtils.Api.Impl;
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
            IServiceProvider minecraftUtils = new ServiceCollection()
                .AddSingletonMinecraftClient()
                .AddSingletonTaskExecutor()
                .BuildServiceProvider();

            IMinecraftClient minecraftClient = minecraftUtils.GetService<IMinecraftClient>();

            IBotStore botStore = new BotStore();
            IGuildsMonitor guildsMonitor = new GuildsMonitor(botStore);
            IGuildCollection guildsCollection = guildsMonitor.Load();
            IMinecraftPlayerClient mojangClient = new MinecraftPlayerClient();
            IConfig config = new ConfigStore();

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton(botStore)
                .AddSingleton(guildsCollection)
                .AddSingleton(guildsMonitor)
                .AddSingleton(mojangClient)
                .AddSingleton(minecraftClient)
                .AddSingleton(config)
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
