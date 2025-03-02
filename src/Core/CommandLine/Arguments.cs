using Core.Log;
using Core.Setup;
using System.Text.Json;

namespace Core.CommandLine
{
    internal static class Arguments
    {
        private const string _help = "--h";
        private const string _version = "--v";
        private const string _executables = "--e";
        private const string _load = "--l";
        private const string _log = "--log";
        private const string _logTimeStamp = "--lts";
        private const string _fileName = "--fn";
        private const string _fileExtension = "--fe";
        private const string _directory = "--dir";
        private const string _buffer = "--buffer";
        private const string _informationUrl = "--iurl";
        private const string _lifetime = "--lt";
        private const string _timeout = "--timeout";
        private const string _overwrite = "--o";

        private static readonly Command _command =
        [
            new Option(_help, "", "Show Help"),
            new Option(_version, "", "Current app version to check if will be updated"),
            new Option(_executables, "", "Executables to close/open during update"),
            new Option(_load, "<path>", "Path to 'configuration.json' file. If not provided, will try and read from executable location."),
            new Option(_log, "<bool>", "Enable logging"),
            new Option(_logTimeStamp, "<bool>", "Enable timestamp in logs"),
            new Option(_fileName, "<string>", "Name of the file"),
            new Option(_fileExtension, "<string>", "Extension of the file"),
            new Option(_directory, "<string>", "Base directory of the application"),
            new Option(_buffer, "<int>", "Default buffer size for http download"),
            new Option(_informationUrl, "<string>", "Information URL"),
            new Option(_lifetime, "<short>", "Lifetime in minutes"),
            new Option(_timeout, "<short>", "Timeout in seconds"),
            new Option(_overwrite, "<bool>", "Overwrite existing files")
        ];

        public static Configuration? Read(string[] args)
        {
            Dictionary<string, string?>? parsedOptions = ParseArguments(args);

            if (parsedOptions is null)
            {
                Console.WriteLine($"Could not parse arguments.");
                return null;
            }

            ShowHelp(parsedOptions);

            if (TryLoadFromFile(parsedOptions, out Configuration? configuration))
            {
                return configuration;
            }

            return new Configuration
            {
                Version = GetOptionValue<string>(parsedOptions, _version),
                Executables = GetOptionValue<string>(parsedOptions, _executables),
                LogEnabled = GetOptionValue<bool>(parsedOptions, _log),
                LogTimeStamp = GetOptionValue<bool>(parsedOptions, _logTimeStamp),
                FileName = GetOptionValue<string>(parsedOptions, _fileName),
                FileExtension = GetOptionValue<string>(parsedOptions, _fileExtension),
                ApplicationBaseDirectory = GetOptionValue<string>(parsedOptions, _directory),
                DefaultBufferSize = GetOptionValue<int>(parsedOptions, _buffer),
                InformationURL = GetOptionValue<string>(parsedOptions, _informationUrl),
                LifeTimeInMinutes = GetOptionValue<short>(parsedOptions, _lifetime),
                TimeoutInSeconds = GetOptionValue<short>(parsedOptions, _timeout),
                Overwrite = GetOptionValue<bool>(parsedOptions, _overwrite)
            };
        }

        private static void ShowHelp(Dictionary<string, string?> parsedOptions)
        {
            if (parsedOptions.TryGetValue(_help, out _) == false)
            {
                return;
            }

            Console.WriteLine("Usage: WinBootStrapper [option] <value>");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Available options are:");
            foreach (Option item in _command.Options)
            {
                Console.WriteLine($"  {item.OptionValue,-30} {item.AcceptedValue,-8} {item.Description}");
            }
            Environment.Exit(0);
        }

        private static bool TryLoadFromFile(Dictionary<string, string?> parsedOptions, out Configuration? configuration)
        {
            configuration = null;

            if (parsedOptions.TryGetValue(_load, out string? path) == false)
            {
                return false;
            }

            string fullPath = string.IsNullOrWhiteSpace(path) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration.json") : path;

            if (File.Exists(fullPath) == false)
            {
                LogWritter.Error($"Could not find configuration file at '{fullPath}'.", true);
                return false;
            }

            configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(fullPath));

            return configuration is not null;
        }

        private static Dictionary<string, string?>? ParseArguments(string[] args)
        {
            Dictionary<string, string?> parsedOptions = [];
            
            for (int i = 0; i < args.Length; i++)
            {
                Option? option = _command.Options.FirstOrDefault(o => o.OptionValue == args[i]);
                if (option is null)
                {
                    continue;
                }

                if (i + 1 < args.Length && args[i + 1].StartsWith("--") == false)
                {
                    parsedOptions[option.Value.OptionValue] = args[i + 1];
                    i++;
                    continue;
                }

                parsedOptions[option.Value.OptionValue] = null;
            }

            return parsedOptions;
        }

        private static T? GetOptionValue<T>(Dictionary<string, string?> parsedOptions, string optionName)
        {
            if (parsedOptions.TryGetValue(optionName, out string? value))
            {
                if (value == null)
                {
                    return default;
                }

                if (typeof(T) == typeof(bool))
                {
                    return (T)(object)(value == "1");
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            return default;
        }
    }
}
