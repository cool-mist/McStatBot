using System;

namespace McStatBot.Core.Guild.Impl
{
    internal class GuildDetails : IGuildDetails
    {
        public string Name { get; set; }

        public DateTime LastActive { get; set; }
    }
}
