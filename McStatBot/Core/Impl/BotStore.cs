using McStatBot.Core.Guild;
using McStatBot.Core.Impl.InternalModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace McStatBot.Core.Impl
{
    public class BotStore : IBotStore
    {
        private string StoreRoot;
        private string GuildsPath;

        public BotStore(string location = ".mcstatbot")
        {
            this.StoreRoot = location;
            this.GuildsPath = $"{StoreRoot}/guilds.json";

            CreateBotStore();
            CreateFile(GuildsPath);
        }

        public IEnumerator<IGuildDetails> ReadGuilds()
        {
            if (File.Exists(GuildsPath))
            {
                try
                {
                    var content = File.ReadAllText(GuildsPath);
                    var guilds = JsonConvert.DeserializeObject<List<GuildDetailsInternal>>(content);

                    if (guilds == null)
                    {
                        return Enumerable.Empty<IGuildDetails>().GetEnumerator();
                    }

                    return guilds.GetEnumerator();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception reading guilds information" + e);
                    return Enumerable.Empty<IGuildDetails>().GetEnumerator();
                }
            }

            return Enumerable.Empty<IGuildDetails>().GetEnumerator();
        }

        public void WriteGuilds(IEnumerator<IGuildDetails> guildDetails)
        {
            var guilds = new List<IGuildDetails>();

            while (guildDetails.MoveNext())
            {
                guilds.Add(guildDetails.Current);
            }

            var content = JsonConvert.SerializeObject(guilds);
            File.WriteAllText(GuildsPath, content);
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

        private void CreateFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                Console.WriteLine($"File {fileName} already exists");
                return;
            }

            File.Create(fileName, 100, FileOptions.SequentialScan).Close();
        }
    }
}