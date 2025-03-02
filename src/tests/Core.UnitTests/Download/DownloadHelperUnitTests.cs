using Core.Download;

namespace Core.UnitTests.Download
{
    public class DownloadHelperUnitTests
    {
        private const string _downloadURL = "http://dl.dropboxusercontent.com/scl/fi/non0pzsqa3caf6a85cwtu/DownloadTest.zip?rlkey=lnhqrert0hg5obchot1f4h0cl&st=qhdebq3p&dl=0";
        private const short _defaultLifeTimeInMinutes = 5;
        private const short _defaultTimeOutInSeconds = 30;

        [Fact(DisplayName = "Should get filezise")]
        [Trait("Download", "Helper")]
        public async Task GetFileSizeAsync_ShouldGetFileSizeAsync()
        {
            //Arrange

            //Act
            HttpResponseMessage? response = await DownloadHelper.GetHttpResponseAsync(_downloadURL, TimeSpan.FromMinutes(_defaultLifeTimeInMinutes), TimeSpan.FromSeconds(_defaultTimeOutInSeconds));
            long fileSize = response!.Content.Headers.ContentLength.GetValueOrDefault(0);

            //Assert
            Assert.True(fileSize > 0);
        }

        [Fact(DisplayName = "Should not get filezise")]
        [Trait("Download", "Helper")]
        public async Task GetFileSizeAsync_ShouldNotGetFileSizeAsync()
        {
            //Arrange

            //Act
            HttpResponseMessage? response = await DownloadHelper.GetHttpResponseAsync("invalid URL", TimeSpan.FromMinutes(_defaultLifeTimeInMinutes), TimeSpan.FromSeconds(_defaultTimeOutInSeconds));

            //Assert
            Assert.Null(response);
        }
    }
}
