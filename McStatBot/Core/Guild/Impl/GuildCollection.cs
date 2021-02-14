using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace McStatBot.Core.Guild.Impl
{
    internal class GuildCollection : IGuildCollection
    {

        private Dictionary<string, IGuildDetails> guildDetails;

        public GuildCollection(IEnumerator<IGuildDetails> guildDetails)
        {
            this.guildDetails = new Dictionary<string, IGuildDetails>(StringComparer.OrdinalIgnoreCase);

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

        public IGuildDetails GetGuild(string guildName)
        {
            if (guildDetails.TryGetValue(guildName, out var guild))
            {
                return guild;
            }

            return null;
        }

        public Task Trace(string guildName)
        {
            if (guildDetails.TryGetValue(guildName, out var guild))
            {
                guild.LastActive = DateTime.Now;
                return Task.CompletedTask;
            }

            this.guildDetails.Add(guildName, new GuildDetails()
            {
                Name = guildName,
                LastActive = DateTime.Now
            });

            return Task.CompletedTask;
        }
    }
}
