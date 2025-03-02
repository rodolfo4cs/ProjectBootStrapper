using Core.Setup;

namespace Core.UnitTests.Setup
{
    public class ConfigurationHelperUnitTests
    {
        private const string _setupfolder = "Setup";
        private const string _oldFileName = "configuration.json";
        private const string _newFileName = "configuration.temp";

        [Fact(DisplayName = "Should try to read a configuration from json returning true")]
        [Trait("Configuration", "Configuration")]
        public void TryReadConfiguration_ShouldReadFromJSON_Return_True()
        {
            //Arrange

            //Act
            bool couldRead = ConfigurationHelper.TryReadConfiguration(out _);

            //Assert
            Assert.True(couldRead);
        }

        [Fact(DisplayName = "Should try to read a configuration from json returning false")]
        [Trait("Configuration", "Configuration")]
        public void TryReadConfiguration_ShouldReadFromJSON_Return_False()
        {
            //Arrange
            string oldFilePath = Path.Combine(AppContext.BaseDirectory, _setupfolder, _oldFileName);
            string newFilePath = Path.Combine(AppContext.BaseDirectory, _setupfolder, _newFileName);
            File.Move(oldFilePath, newFilePath);

            //Act
            bool couldRead = ConfigurationHelper.TryReadConfiguration(out _);
            File.Move(newFilePath, oldFilePath);

            //Assert
            Assert.False(couldRead);
        }

        [Fact(DisplayName = "Should create configuration object and try to read it from json and check its values")]
        [Trait("Configuration", "Configuration")]
        public void TryReadConfiguration_ShouldCreateAndReadFromJSON()
        {
            //Arrange

            //Act
            bool couldRead = ConfigurationHelper.TryReadConfiguration(out Configuration configuration);

            //Assert
            Assert.True(couldRead);
            Assert.Equal(string.Empty, configuration.FileName);
        }
    }
}
