using McStatBot.Core.Guild;
using System;

namespace McStatBot.Core.Impl.InternalModels
{
    internal class GuildDetailsInternal : IGuildDetails
    {
        public string Name { get; set; }

        public string DefaultServerName { get; set; }
        public DateTime LastActive { get; set; }
    }
}
