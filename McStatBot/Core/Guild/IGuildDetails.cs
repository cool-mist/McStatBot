using System;

namespace McStatBot.Core.Guild
{
    public interface IGuildDetails
    {
        string Name { get; }

        DateTime LastActive { get; }
    }
}
