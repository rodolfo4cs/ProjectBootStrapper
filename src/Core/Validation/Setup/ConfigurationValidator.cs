using Core.Setup;
using Core.Util.Validation;

namespace Core.Validation.Setup
{
    internal sealed class ConfigurationValidator
    {
        public Dictionary<string, string> Validate(Configuration config)
        {
            Dictionary<string, string> errors = [];

            if (config.LogEnabled is null)
            {
                errors.Add(nameof(config.LogEnabled), "'LogEnabled' is required");
            }

            if (string.IsNullOrWhiteSpace(config.FileName))
            {
                errors.Add(nameof(config.FileName), "'FileName' is required");
            }

            if (string.IsNullOrWhiteSpace(config.FileExtension))
            {
                errors.Add(nameof(config.FileExtension), "'FileExtension' is required");
            }

            if (string.IsNullOrWhiteSpace(config.ApplicationBaseDirectory))
            {
                errors.Add(nameof(config.ApplicationBaseDirectory), "'ApplicationBaseDirectory' is required");
            }

            if (config.DefaultBufferSize is null || config.DefaultBufferSize <= 0)
            {
                errors.Add(nameof(config.DefaultBufferSize), "'DefaultBufferSize' is required and must be greater than 0");
            }

            if (string.IsNullOrWhiteSpace(config.InformationURL))
            {
                errors.Add(nameof(config.InformationURL), "'InformationURL' is required");
            }
            else if (CustomValidation.BeAValidURL(config.InformationURL) == false)
            {
                errors.Add(nameof(config.InformationURL), "'InformationURL' is not a valid URL");
            }

            if (config.LifeTimeInMinutes is null || config.LifeTimeInMinutes <= 0)
            {
                errors.Add(nameof(config.LifeTimeInMinutes), "'LifeTimeInMinutes' is required and must be greater than 0");
            }

            if (config.TimeoutInSeconds is null || config.TimeoutInSeconds < 30)
            {
                errors.Add(nameof(config.TimeoutInSeconds), "'TimeoutInSeconds' is required and must be equal or greater than 30");
            }

            if (config.Overwrite is null)
            {
                errors.Add(nameof(config.Overwrite), "'Overwrite' is required");
            }

            return errors;
        }
    }
}
