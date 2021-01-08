using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace McStatBot.Impl
{
    internal class McSrvrStat : IMinecraftServerStatsClient, IDisposable
    {

        private readonly string baseUrl = @"https://api.mcsrvstat.us";
        private readonly string apiVersion = "2";
        private readonly HttpClient httpClient;

        public McSrvrStat()
        {
            this.httpClient = HttpClientFactory.Create();
            this.httpClient.BaseAddress = new Uri($"{baseUrl}/{apiVersion}/");
        }

        public async Task<MinecraftServerStat> GetStatus(string serverName)
        {
            using (var httpResponseMessage = await this.httpClient.GetAsync(serverName))
            {
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var httpContent = httpResponseMessage.Content;
                    var minecraftServerStat = await httpContent.ReadAsAsync<MinecraftServerStat>();

                    minecraftServerStat.Icon = $"{baseUrl}/icon/{minecraftServerStat.Hostname}";

                    return minecraftServerStat;
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
