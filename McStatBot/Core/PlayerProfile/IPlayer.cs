using System;

namespace McStatBot.Core.PlayerProfile
{
    public interface IPlayer
    {
        bool Found { get; }
        string Name { get; }
        Guid Id { get; }
        string Skin { get; }
        string SkinType { get; }
        bool Legacy { get; }
        bool Demo { get; }
        IPlayerNames[] Names { get; }
    }

    public interface IPlayerNames
    {
        string Name { get; }

        DateTime Since { get; }
    }
}
