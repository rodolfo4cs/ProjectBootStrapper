using Core.CommandLine;
using Core.Log;
using Core.Validation.Setup;
using System.Text.Json;

namespace Core.Setup
{
    internal static class ConfigurationHelper
    {
        private const string _fileName = "configuration.json";
        private const string _Setupfolder = "Setup";

        public static bool TryReadConfiguration(out Configuration? configuration)
        {
            try
            {
                string filePath = Path.Combine(AppContext.BaseDirectory, _Setupfolder, _fileName);

                if (File.Exists(filePath) == false)
                {
                    throw new FileNotFoundException($"File not found: {filePath}");
                }

                string jsonText = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(jsonText))
                {
                    throw new NullReferenceException($"File {filePath} is empty");
                }

                Configuration? nullableConfiguration = JsonSerializer.Deserialize<Configuration>(jsonText);

                if (nullableConfiguration is null)
                {
                    throw new NullReferenceException($"Could not read configuration file {filePath}");
                }

                configuration = nullableConfiguration;
                return true;
            }
            catch (Exception ex)
            {
                LogWritter.Error($"Error trying to read configuration file: {ex.Message}");
            }

            configuration = null;
            return false;
        }

        public static Configuration GetConfiguration(ref string[] args)
        {
            if (args.Length == 0)
            {
                LogWritter.Log($"No arguments passed, showing help");
                args = ["--h"];
            }

            Configuration? configuration = Arguments.Read(args);

            if (configuration is null)
            {
                LogWritter.Log($"Could not create/read a configuration based on the passed arguments.");
                Environment.Exit(1);
            }

            ConfigurationValidator validator = new();
            Dictionary<string, string> result = validator.Validate(configuration);

            if (result.Count > 0)
            {
                LogWritter.Error($"Configuration is not valid, check log for more details.", true);
                foreach (KeyValuePair<string, string> error in result)
                {
                    LogWritter.Error($"  > {error.Value}", true);
                }
                Environment.Exit(1);
            }

            return configuration;
        }
    }
}
