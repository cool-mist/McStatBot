using System.Collections.Generic;
using System.Threading.Tasks;

namespace McStatBot.Core.Guild
{
    public interface IGuildCollection
    {
        Task Trace(string guildName);

        IEnumerator<IGuildDetails> GetAllGuilds();

        IGuildDetails GetGuild(string guildName);
    }
}
