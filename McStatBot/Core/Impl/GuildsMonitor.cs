using System.Collections.Generic;

namespace McStatBot.Core.Impl
{
    public class GuildsMonitor : IGuildsMonitor
    {
        private IGuildCollection guildCollection;
        private IBotStore botStore;

        public GuildsMonitor(IBotStore botStore)
        {
            this.botStore = botStore;
        }

        public IGuildCollection Load()
        {
            IEnumerator<GuildDetails> guildDetails = botStore.ReadGuilds();

            this.guildCollection = new GuildCollection(guildDetails);

            return this.guildCollection;
        }

        public void Persist()
        {
            this.botStore.WriteGuilds(this.guildCollection.GetAllGuilds());
        }
    }
}
