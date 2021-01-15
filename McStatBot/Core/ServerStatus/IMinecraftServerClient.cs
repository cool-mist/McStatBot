using System;
using System.Threading.Tasks;

namespace McStatBot.Core.ServerStatus
{
    public interface IMinecraftServerClient : IDisposable
    {
        Task<IMinecraftServer> GetServerStatus(string serverName);
    }
}