using System;
using System.Threading.Tasks;

namespace McStatBot
{
    public interface IMinecraftServerStatsClient : IDisposable
    {
        Task<MinecraftServerStat> GetStatus(string serverName);
    }
}
