using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace McStatBot.Core.Impl
{
    class BotStore : IBotStore
    {
        private string StoreRoot;
        private string GuildsPath;

        public BotStore(string location = ".mcstatbot")
        {
            this.StoreRoot = location;
            this.GuildsPath = $"{StoreRoot}/guilds.json";

            CreateBotStore();
        }

        private void CreateBotStore()
        {
            if (File.Exists(StoreRoot))
            {
                var attributes = File.GetAttributes(StoreRoot);
                if (!attributes.HasFlag(FileAttributes.Directory))
                {
                    throw new Exception($"Folder {StoreRoot} already exists and is not a directory");
                }

                return;
            }

            if (Directory.Exists(StoreRoot))
            {
                Console.WriteLine($"Store {StoreRoot} already exists");
                return;
            }

            var directory = Directory.CreateDirectory(StoreRoot);
            Console.WriteLine($"Created {directory.FullName}");
        }

        private void CreateGuildsJson()
        {
            File.Create(GuildsPath, 100, FileOptions.SequentialScan);
        }

        public IEnumerator<GuildDetails> ReadGuilds()
        {
            if (File.Exists(GuildsPath))
            {
                try
                {
                    var content = File.ReadAllText(GuildsPath);
                    var guilds = JsonConvert.DeserializeObject<List<GuildDetails>>(content);

                    if (guilds == null)
                    {
                        return Enumerable.Empty<GuildDetails>().GetEnumerator();
                    }

                    return guilds.GetEnumerator();
                }
                catch (Exception e)
                {
                    return Enumerable.Empty<GuildDetails>().GetEnumerator();
                }
            }

            CreateGuildsJson();
            return Enumerable.Empty<GuildDetails>().GetEnumerator();
        }

        public void WriteGuilds(IEnumerator<GuildDetails> guildDetails)
        {
            var guilds = new List<GuildDetails>();

            while (guildDetails.MoveNext())
            {
                guilds.Add(guildDetails.Current);
            }

            var content = JsonConvert.SerializeObject(guilds);
            File.WriteAllText($"{StoreRoot}/guilds.json", content);
        }
    }
}