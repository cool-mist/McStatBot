using System;

namespace McStatBot.Core.Config.Impl
{
    internal class ConfigStore : IConfig
    {
        public string Get(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
