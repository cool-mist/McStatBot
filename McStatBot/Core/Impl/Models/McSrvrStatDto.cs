using Newtonsoft.Json;

namespace McStatBot.Core.Impl.Models
{
    internal class McSrvrStatDto
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("online")]
        public bool Online { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("players")]
        public PlayersStat Players { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("port")]
        public int? Port { get; set; }

        [JsonProperty("motd")]
        public Motd Motd { get; set; }

        public string Icon { get; private set; }

        public void SetIcon(string iconUrl)
        {
            this.Icon = iconUrl;
        }
    }

    internal class Motd
    {
        [JsonProperty("raw")]
        public string[] Raw { get; set; }

        [JsonProperty("clean")]
        public string[] Clean { get; set; }

        [JsonProperty("html")]
        public string[] Html { get; set; }
    }

    internal class PlayersStat
    {
        [JsonProperty(PropertyName = "max")]
        public int Max { get; set; }

        [JsonProperty(PropertyName = "online")]
        public int Online { get; set; }

    }
}
