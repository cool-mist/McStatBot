namespace McStatBot.Core
{
    interface IGuildsMonitor
    {
        IGuildCollection Load();
        void Persist();
    }
}
