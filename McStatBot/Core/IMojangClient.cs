using System;
using System.Threading.Tasks;

namespace McStatBot.Core
{
    public interface IMojangClient : IDisposable
    {
        public Task<IPlayer> GetPlayer(string playerName);
    }
}