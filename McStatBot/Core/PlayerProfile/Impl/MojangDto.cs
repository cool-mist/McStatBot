using Newtonsoft.Json;

namespace McStatBot.Core.PlayerProfile.Impl
{
    internal class PlayerDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("legacy")]
        public bool? Legacy { get; set; }

        [JsonProperty("demo")]
        public bool? Demo { get; set; }

        [JsonProperty("properties")]
        public ProfileProperties[] Properties { get; set; }

        public Texture Texture { get; private set; }

        public void SetTexture(Texture texture)
        {
            this.Texture = texture;
        }
    }

    internal class NameItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("changedToAt")]
        public long? ChangedToAt { get; set; }
    }

    internal class ProfileProperties
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    internal class Texture
    {
        [JsonProperty("textures")]
        public TextureData TextureData { get; set; }
    }

    internal class TextureData
    {
        [JsonProperty("SKIN")]
        public SkinTextureData Skin { get; set; }
    }

    internal class SkinTextureData
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("metadata")]
        public SkinTextureMetadata Metadata { get; set; }
    }

    internal class SkinTextureMetadata
    {
        [JsonProperty("model")]
        public string Model { get; set; }
    }
}
