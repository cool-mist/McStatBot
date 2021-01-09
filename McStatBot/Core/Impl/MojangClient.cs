﻿using McStatBot.Core;
using McStatBot.Core.Impl.Models;
using McStatBot.Models;
using McStatBot.Utils;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace McStatBot.Impl
{
    /// <summary>
    /// https://wiki.vg/Mojang_API
    /// </summary>
    internal class MojangClient : IMojangClient
    {

        private readonly string profileByPlayerNameUri = @"https://api.mojang.com/profiles/minecraft";
        private readonly string profileByUuidUri = @"https://sessionserver.mojang.com/session/minecraft/profile/{0}";
        private readonly string nameHistoryByUuidUri = @"https://api.mojang.com/user/profiles/{0}/names";
        private readonly HttpClient httpClient;

        public MojangClient()
        {
            this.httpClient = HttpClientFactory.Create();
        }

        public async Task<IPlayer> GetPlayer(string playerName)
        {

            string data = JsonConvert.SerializeObject(new string[1] { playerName });

            using (var httpResponseMessage = await this.httpClient.PostAsync(
                profileByPlayerNameUri,
                new StringContent(data, Encoding.UTF8, "application/json")))
            {
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                    var playerSlimDetails = JsonConvert.DeserializeObject<PlayerDto[]>(responseString).First();

                    var playerTextureDetails = await GetPlayerTextures(playerSlimDetails.Id);

                    if (playerTextureDetails == null)
                    {
                        return null;
                    }

                    var playerNameHistory = await GetPlayerNameHistory(playerSlimDetails.Id);

                    return new Player()
                    {
                        Name = playerSlimDetails.Name,
                        Id = Guid.Parse(playerSlimDetails.Id),
                        Legacy = playerSlimDetails.Legacy ?? false,
                        Demo = playerSlimDetails.Demo ?? false,
                        Skin = playerTextureDetails.Texture.TextureData.Skin.Url,
                        SkinType = playerTextureDetails.Texture.TextureData.Skin.Metadata?.Model ?? "square",
                        Names = playerNameHistory.Select(n => new PlayerName()
                        {
                            Name = n.Name,
                            Since = n.ChangedToAt.ToDateTime()
                        }).ToArray()
                    };
                }
            }

            return null;
        }

        private async Task<NameItem[]> GetPlayerNameHistory(string id)
        {
            using (var httpResponseMessage = await this.httpClient.GetAsync(string.Format(nameHistoryByUuidUri, id)))
            {
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<NameItem[]>(responseString);
                }
            }

            return null;
        }

        private async Task<PlayerDto> GetPlayerTextures(string id)
        {
            using (var httpResponseMessage = await this.httpClient.GetAsync(string.Format(profileByUuidUri, id)))
            {
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var player = await httpResponseMessage.Content.ReadAsAsync<PlayerDto>();
                    var textureBytes = Convert.FromBase64String(player.Properties.First().Value);
                    var textureJson = Encoding.UTF8.GetString(textureBytes);
                    var texture = JsonConvert.DeserializeObject<Texture>(textureJson);

                    player.SetTexture(texture);

                    return player;
                }
            }

            return null;
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