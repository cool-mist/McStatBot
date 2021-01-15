using System;
using System.Collections.Generic;

namespace McStatBot.Core.Guild.Impl
{
    internal class GuildCollection : IGuildCollection
    {

        private Dictionary<string, IGuildDetails> guildDetails;

        public GuildCollection(IEnumerator<IGuildDetails> guildDetails)
        {
            this.guildDetails = new Dictionary<string, IGuildDetails>();

            while (guildDetails.MoveNext())
            {
                var guild = guildDetails.Current;
                this.guildDetails.Add(guild.Name, guild);
            }
        }

        public IEnumerator<IGuildDetails> GetAllGuilds()
        {
            return guildDetails.Values.GetEnumerator();
        }

        public void Trace(string guildName)
        {
            if (guildDetails.ContainsKey(guildName))
            {
                guildDetails.Remove(guildName);
            }

            this.guildDetails.Add(guildName, new GuildDetails()
            {
                Name = guildName,
                LastActive = DateTime.Now
            });
        }
    }
}
