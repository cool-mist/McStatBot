namespace McStatBot.Core
{
    public interface IMinecraftServer
    {
        public string Hostname { get; }
        public string Version { get; }
        public string Motd { get; }
        public int MaxPlayers { get; }
        public int OnlinePlayers { get; }
        public bool Online { get; }
        public string Icon { get; }
    }
}