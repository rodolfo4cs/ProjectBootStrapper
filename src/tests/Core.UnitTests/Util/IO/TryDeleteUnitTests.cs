using Core.Util.IO;

namespace Core.UnitTests.Util.IO
{
    public class TryDeleteUnitTests
    {
        [Fact(DisplayName = "Should try and delete a file from disk returning true")]
        [Trait("Util", "IO")]
        public void Delete_ShouldTryDeleteTextFile_True()
        {
            //Arrange
            string filePath = Path.Combine(AppContext.BaseDirectory, "TryDeleteTrue.txt");
            File.WriteAllText(filePath, "This file will be deleted");

            //Act
            bool confirmation = TryIO.Delete(filePath);

            //Assert
            Assert.False(File.Exists(filePath));
            Assert.True(confirmation);
        }

        [Fact(DisplayName = "Should try and delete a file from disk returning false")]
        [Trait("Util", "IO")]
        public void Delete_ShouldTryDeleteTextFile_False()
        {
            //Arrange
            string filePath = Path.Combine(AppContext.BaseDirectory, "TryDeleteFalse.txt");
            File.WriteAllText(filePath, "This file will NOT be deleted");

            //Act
            using FileStream fs = FileLocker.LockFile(filePath);
            bool confirmation = TryIO.Delete(filePath);

            //Assert
            Assert.True(File.Exists(filePath));
            Assert.False(confirmation);
        }
    }

    public static class FileLocker
    {
        public static FileStream LockFile(string filePath)
        {
            return new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        }
    }
}
