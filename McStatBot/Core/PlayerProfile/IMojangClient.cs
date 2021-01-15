using System;
using System.Threading.Tasks;

namespace McStatBot.Core.PlayerProfile
{
    public interface IMojangClient : IDisposable
    {
        public Task<IPlayer> GetPlayer(string playerName);
    }
}