using McStatBot.Core;
using McStatBot.Core.Impl.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace McStatBot.Impl
{
    internal class McSrvrStatClient : IMinecraftServerClient, IDisposable
    {
        private readonly string baseUrl = @"https://api.mcsrvstat.us";
        private readonly string apiVersion = "2";
        private readonly HttpClient httpClient;

        public McSrvrStatClient()
        {
            this.httpClient = HttpClientFactory.Create();
            this.httpClient.BaseAddress = new Uri($"{baseUrl}/{apiVersion}/");
        }

        public async Task<IMinecraftServer> GetServerStatus(string serverName)
        {
            using (var httpResponseMessage = await this.httpClient.GetAsync(serverName))
            {
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var httpContent = httpResponseMessage.Content;
                    var minecraftServerStat = await httpContent.ReadAsAsync<McSrvrStatDto>();

                    return new MinecraftServer()
                    {
                        Hostname = minecraftServerStat.Hostname,
                        Version = minecraftServerStat.Version,
                        OnlinePlayers = minecraftServerStat.Players.Online,
                        MaxPlayers = minecraftServerStat.Players.Max,
                        Motd = minecraftServerStat.Motd.Clean.First(),
                        Online = minecraftServerStat.Online,
                        Icon = $"{baseUrl}/icon/{minecraftServerStat.Hostname}"
                    };
                }
                else
                {
                    Console.WriteLine($"Fetch failed for {serverName} with {httpResponseMessage.StatusCode}");
                    return null;
                }
            }
        }

        public void Dispose()
        {
            try
            {
                if (this.httpClient != null)
                {
                    this.httpClient.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
