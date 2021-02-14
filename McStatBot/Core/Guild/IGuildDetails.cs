using System;

namespace McStatBot.Core.Guild
{
    public interface IGuildDetails
    {
        string Name { get; }

        string DefaultServerName { get; set; } //TODO : Fix access.

        DateTime LastActive { get; set; }
    }
}
