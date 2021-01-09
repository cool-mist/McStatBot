using System;
using System.Threading.Tasks;

namespace McStatBot.Core
{
    public interface IMinecraftServerClient : IDisposable
    {
        Task<IMinecraftServer> GetServerStatus(string serverName);
    }
}