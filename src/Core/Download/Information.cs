using System.Text.Json.Serialization;

namespace Core.Download
{
    internal sealed class Information
    {
        [JsonPropertyName("version")]
        public required string Version { get; set; }

        [JsonPropertyName("releaseDate")]
        public required DateTime ReleaseDate { get; set; }

        [JsonPropertyName("downloadUrl")]
        public required string DownloadUrl { get; set; }

        [JsonIgnore]
        public Version NewVersion { get { return new(Version); } }
    }
}
