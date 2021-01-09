using System;

namespace McStatBot.Core.Impl.Models
{
    internal class Player : IPlayer
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public string Skin { get; set; }

        public string SkinType { get; set; }

        public bool Legacy { get; set; }

        public bool Demo { get; set; }

        public IPlayerNames[] Names { get; set; }
    }

    internal class PlayerName : IPlayerNames
    {
        public string Name { get; set; }

        public DateTime Since { get; set; }
    }
}
