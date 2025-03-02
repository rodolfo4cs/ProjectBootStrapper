using System.Text.Json.Serialization;

namespace Core.Setup
{
    internal sealed class Configuration
    {
        private const string _downloadsFolder = "downloads";
        private const string _extractedFolder = "extracted";
        private const short _defaultLifeTimeInMinutes = 5;
        private const short _defaultTimeOutInSeconds = 30;

        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("executables")]
        public string? Executables { get; set; }

        [JsonPropertyName("logEnabled")]
        public bool? LogEnabled { get; set; }

        [JsonPropertyName("logTimeStamp")]
        public bool? LogTimeStamp { get; set; }

        [JsonPropertyName("fileName")]
        public string? FileName { get; set; }

        [JsonPropertyName("fileExtension")]
        public string? FileExtension { get; set; }

        [JsonPropertyName("applicationBaseDirectory")]
        public string? ApplicationBaseDirectory { get; set; }

        [JsonPropertyName("defaultBufferSize")]
        public int? DefaultBufferSize { get; set; }

        [JsonPropertyName("informationUrl")]
        public string? InformationURL { get; set; }

        [JsonPropertyName("lifeTimeInMinutes")]
        public short? LifeTimeInMinutes { get; set; }

        [JsonPropertyName("timeoutInSeconds")]
        public short? TimeoutInSeconds { get; set; }

        [JsonPropertyName("overwrite")]
        public bool? Overwrite { get; set; }

        [JsonIgnore]
        public Version CurrentVersion { get { return new(Version ?? "0.0.0.1"); } }

        [JsonIgnore]
        public TimeSpan LifeTimeTimeSpan { get { return TimeSpan.FromMinutes(LifeTimeInMinutes.GetValueOrDefault(_defaultLifeTimeInMinutes)); } }

        [JsonIgnore]
        public TimeSpan TimeOutTimeSpan { get { return TimeSpan.FromSeconds(TimeoutInSeconds.GetValueOrDefault(_defaultTimeOutInSeconds)); } }

        [JsonIgnore]
        public string? FileNameWithExtension { get { return FileName + FileExtension; } }

        [JsonIgnore]
        public string DownloadPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ApplicationBaseDirectory))
                {
                    return "";
                }

                return Path.Combine(ApplicationBaseDirectory, _downloadsFolder);
            }
        }

        [JsonIgnore]
        public string DownloadedFilePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ApplicationBaseDirectory) || string.IsNullOrWhiteSpace(FileNameWithExtension) || string.IsNullOrWhiteSpace(FileExtension))
                {
                    return "";
                }

                return Path.Combine(DownloadPath, FileNameWithExtension);
            }
        }

        [JsonIgnore]
        public string ExtractPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DownloadPath))
                {
                    return "";
                }

                return Path.Combine(DownloadPath, _extractedFolder);
            }
        }
    }
}
