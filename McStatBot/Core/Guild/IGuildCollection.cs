using System.Collections.Generic;

namespace McStatBot.Core.Guild
{
    public interface IGuildCollection
    {
        void Trace(string guildName);

        IEnumerator<IGuildDetails> GetAllGuilds();
    }
}
