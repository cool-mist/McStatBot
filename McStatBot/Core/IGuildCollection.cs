using System;
using System.Collections.Generic;

namespace McStatBot.Core
{
    public interface IGuildCollection
    {
        void Trace(string guildName);

        IEnumerator<GuildDetails> GetAllGuilds();
    }

    public class GuildDetails
    {
        public string Name { get; set; }
        public DateTime LastActive { get; set; }
    }
}
