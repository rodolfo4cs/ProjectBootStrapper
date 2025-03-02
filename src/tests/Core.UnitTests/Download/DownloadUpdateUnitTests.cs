using Core.Download;
using Core.Setup;

namespace Core.UnitTests.Download
{
    public class DownloadUpdateUnitTests
    {
        private const string _downloadURL = "http://dl.dropboxusercontent.com/scl/fi/non0pzsqa3caf6a85cwtu/DownloadTest.zip?rlkey=lnhqrert0hg5obchot1f4h0cl&st=qhdebq3p&dl=0";
        private const string _informationURL = "https://pastebin.com/raw/DEsNCfDv";

        private readonly Configuration configuration;
        private readonly Information information;
        private readonly long? _fileSize = 15505013; //FileSize set mannuallly

        public DownloadUpdateUnitTests()
        {
            configuration = new()
            {
                FileName = "DefaultName",
                FileExtension = ".zip",
                ApplicationBaseDirectory = AppContext.BaseDirectory,
                DefaultBufferSize = 8192,
                InformationURL = _informationURL,
                LifeTimeInMinutes = 5,
                TimeoutInSeconds = 30
            };

            information = new()
            {
                Version = "1.0.0",
                ReleaseDate = DateTime.Now,
                DownloadUrl = _downloadURL
            };
        }

        [Fact(DisplayName = "Should download a file to disk")]
        [Trait("Download", "Download")]
        public async Task DownloadHttpAsync_ShouldDownloadFile_StartToFinish()
        {
            //Arrange
            configuration.FileName = "DownloadHttpAsync_ShouldDownloadFile_StartToFinish";
            configuration.Overwrite = true;
            File.Delete(configuration.DownloadedFilePath);

            //Act
            await DownloadUpdate.DownloadHttpAsync(configuration, information);

            //Assert
            Assert.NotNull(_fileSize);
            Assert.True(File.Exists(configuration.DownloadedFilePath));
            Assert.Equal(_fileSize, new FileInfo(configuration.DownloadedFilePath).Length);
        }

        [Fact(DisplayName = "Should download a file to disk then delete it")]
        [Trait("Download", "Download")]
        public async Task DownloadHttpAsync_ShouldDownloadFile_StartToFinish_DeleteWhenDone()
        {
            //Arrange
            configuration.FileName = "DownloadHttpAsync_ShouldDownloadFile_StartToFinish_DeleteWhenDone";
            configuration.Overwrite = true;

            //Act
            await DownloadUpdate.DownloadHttpAsync(configuration, information);

            //Assert
            Assert.NotNull(_fileSize);
            Assert.True(File.Exists(configuration.DownloadedFilePath));
            Assert.Equal(_fileSize, new FileInfo(configuration.DownloadedFilePath).Length);
            File.Delete(configuration.DownloadedFilePath);
            Assert.False(File.Exists(configuration.DownloadedFilePath));
        }

        [Fact(DisplayName = "Should continue a file download to disk")]
        [Trait("Download", "Download")]
        public async Task DownloadHttpAsync_ShouldDownloadFile_AlreadyStartedToFinish()
        {
            //Arrange
            configuration.FileName = "DownloadHttpAsync_ShouldDownloadFile_AlreadyStartedToFinish";
            configuration.Overwrite = false;
            File.Delete(configuration.DownloadedFilePath);

            //Act
            File.Create("stop_download.test").Close();
            await DownloadUpdate.DownloadHttpAsync(configuration, information);
            File.Delete("stop_download.test");
            await DownloadUpdate.DownloadHttpAsync(configuration, information);

            //Assert
            Assert.NotNull(_fileSize);
            Assert.True(File.Exists(configuration.DownloadedFilePath));
            Assert.Equal(_fileSize, new FileInfo(configuration.DownloadedFilePath).Length);
        }

        [Fact(DisplayName = "Should continue a file download to disk then delete it")]
        [Trait("Download", "Download")]
        public async Task DownloadHttpAsync_ShouldDownloadFile_AlreadyStartedToFinish_DeleteWhenDone()
        {
            //Arrange
            configuration.FileName = "DownloadHttpAsync_ShouldDownloadFile_AlreadyStartedToFinish_DeleteWhenDone";
            configuration.Overwrite = false;

            //Act
            File.Create("stop_download.test").Close();
            await DownloadUpdate.DownloadHttpAsync(configuration, information);
            File.Delete("stop_download.test");
            await DownloadUpdate.DownloadHttpAsync(configuration, information);

            //Assert
            Assert.NotNull(_fileSize);
            Assert.True(File.Exists(configuration.DownloadedFilePath));
            Assert.Equal(_fileSize, new FileInfo(configuration.DownloadedFilePath).Length);
            File.Delete(configuration.DownloadedFilePath);
            Assert.False(File.Exists(configuration.DownloadedFilePath));
        }

        [Fact(DisplayName = "Should download a file to disk twice with overwrite enabled")]
        [Trait("Download", "Download")]
        public async Task DownloadHttpAsync_ShouldDownloadFile_StartToFinish_Twice_WithOverwrite_True()
        {
            //Arrange
            configuration.FileName = "DownloadHttpAsync_ShouldDownloadFile_StartToFinish_Twice_WithOverwrite_True";
            configuration.Overwrite = true;

            //Act
            await DownloadUpdate.DownloadHttpAsync(configuration, information);
            await DownloadUpdate.DownloadHttpAsync(configuration, information);

            //Assert
            Assert.NotNull(_fileSize);
            Assert.True(File.Exists(configuration.DownloadedFilePath));
            Assert.Equal(_fileSize, new FileInfo(configuration.DownloadedFilePath).Length);
        }

        [Fact(DisplayName = "Should download a file to disk twice with overwrite disabled")]
        [Trait("Download", "Download")]
        public async Task DownloadHttpAsync_ShouldDownloadFile_StartToFinish_Twice_WithOverwrite_False()
        {
            //Arrange
            configuration.FileName = "DownloadHttpAsync_ShouldDownloadFile_StartToFinish_Twice_WithOverwrite_False";
            configuration.Overwrite = false;

            //Act
            await DownloadUpdate.DownloadHttpAsync(configuration, information);
            await DownloadUpdate.DownloadHttpAsync(configuration, information);

            //Assert
            Assert.NotNull(_fileSize);
            Assert.True(File.Exists(configuration.DownloadedFilePath));
            Assert.Equal(_fileSize, new FileInfo(configuration.DownloadedFilePath).Length);
        }
    }
}
