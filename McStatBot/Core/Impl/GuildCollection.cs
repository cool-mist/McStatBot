using System;
using System.Collections.Generic;

namespace McStatBot.Core.Impl
{
    public class GuildCollection : IGuildCollection
    {

        private Dictionary<string, GuildDetails> guildDetails;

        public GuildCollection(IEnumerator<GuildDetails> guildDetails)
        {
            this.guildDetails = new Dictionary<string, GuildDetails>();

            while (guildDetails.MoveNext())
            {
                var guild = guildDetails.Current;
                this.guildDetails.Add(guild.Name, guild);
            }
        }

        public IEnumerator<GuildDetails> GetAllGuilds()
        {
            return guildDetails.Values.GetEnumerator();
        }

        public void Trace(string guildName)
        {
            if (guildDetails.ContainsKey(guildName))
            {
                this.guildDetails[guildName].LastActive = DateTime.Now;
                return;
            }

            this.guildDetails.Add(guildName, new GuildDetails()
            {
                Name = guildName,
                LastActive = DateTime.Now
            });
        }
    }
}
