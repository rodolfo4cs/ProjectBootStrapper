using Core.Setup;
using Core.Validation.Setup;

namespace Core.UnitTests.Validation.Setup
{
    public class ConfigurationValidatorUnitTests
    {
        [Fact(DisplayName = "Should validate configuration object")]
        [Trait("Validation", "Configuration")]
        public void Validate_ShouldValidateConfiguration_ValidURL()
        {
            //Arrange
            Configuration config = new()
            {
                LogEnabled = true,
                FileName = "FileName",
                FileExtension = ".zip",
                ApplicationBaseDirectory = AppContext.BaseDirectory,
                DefaultBufferSize = 1024,
                InformationURL = "https://www.github.com",
                LifeTimeInMinutes = 5,
                TimeoutInSeconds = 30,
                Overwrite = false
            };
            ConfigurationValidator validator = new();

            //Act
            Dictionary<string, string> result = validator.Validate(config);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Should NOT validate configuration object url")]
        [Trait("Validation", "Configuration")]
        public void Validate_ShouldValidateConfiguration_InValidURL()
        {
            //Arrange
            Configuration config = new();
            ConfigurationValidator validator = new();

            //Act
            Dictionary<string, string> result = validator.Validate(config);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count == 9);
        }
    }
}
