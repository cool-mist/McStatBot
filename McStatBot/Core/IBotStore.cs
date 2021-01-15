using System.Collections.Generic;

namespace McStatBot.Core
{
    public interface IBotStore
    {
        void WriteGuilds(IEnumerator<GuildDetails> guildDetails);

        IEnumerator<GuildDetails> ReadGuilds();
    }
}
