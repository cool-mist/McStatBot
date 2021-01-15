using McStatBot.Core.Guild;
using System.Collections.Generic;

namespace McStatBot.Core
{
    public interface IBotStore
    {
        void WriteGuilds(IEnumerator<IGuildDetails> guildDetails);

        IEnumerator<IGuildDetails> ReadGuilds();
    }
}
