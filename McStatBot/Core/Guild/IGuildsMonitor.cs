namespace McStatBot.Core.Guild
{
    interface IGuildsMonitor
    {
        IGuildCollection Load();
        void Persist();
    }
}
