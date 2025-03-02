using Core.Download;
using Core.Setup;

namespace Core.UnitTests.Download
{
    public sealed class DownloadInformationUnitTests
    {
        private const string _informationURL = "https://pastebin.com/raw/DEsNCfDv";

        private readonly Configuration configuration;

        public DownloadInformationUnitTests()
        {
            configuration = new()
            {
                InformationURL = _informationURL
            };
        }

        [Fact(DisplayName = "Should download the download information from a url")]
        [Trait("Download", "Information")]
        public async Task DownloadHttpAsync_ShouldDownloadInformationData()
        {
            //Arrange

            //Act
            Information? information = await DownloadInformation.DownloadHttpAsync(configuration);

            //Assert
            Assert.NotNull(information);
        }
    }
}
