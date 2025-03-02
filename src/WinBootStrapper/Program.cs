using Core.Download;
using Core.Log;
using Core.Setup;
using Core.Util.IO;
using System.Diagnostics;

namespace WinBootStrapper
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //args = [ "--v", "1.0.0.0", "--e", "FakeAppToTest.exe", "--log", "1", "--lts", "1", "--fn", "DownloadedUpdate",  "--fe", ".zip", "--dir", @"C:\Sistemas\MyAppToBeUpdated", "--buffer", "8192", "--iurl", "https://pastebin.com/raw/DEsNCfDv", "--lt", "5", "--timeout", "30", "--o", "0" ]

            LogWritter.Log("Starting application.");
            LogWritter.Log($"Arguments '{string.Join(',', args)}'");

            Configuration configuration = ConfigurationHelper.GetConfiguration(ref args);

            LogWritter.Initialize(configuration.LogEnabled.GetValueOrDefault(true), configuration.LogTimeStamp.GetValueOrDefault(true));

            Information? information = await DownloadInformation.DownloadHttpAsync(configuration);

            if (information is null)
            {
                LogWritter.Log($"Could not download information, check log for more details.", true);
                Environment.Exit(1);
            }

            if (information.NewVersion <= configuration.CurrentVersion)
            {
                LogWritter.Log($"No updates available, current version '{configuration.Version}', latest version '{information.Version}'.");
                Environment.Exit(0);
            }

            await DownloadUpdate.DownloadHttpAsync(configuration, information);

            Compression.Decompress(ECompressionType.Zip, configuration);

            await StopExecutablesPreUpdate(configuration);

            await ApplyUpdate(configuration);

            await StartExecutablesPostUpdate(configuration);
        }

        private static async Task StopExecutablesPreUpdate(Configuration configuration)
        {
            if (configuration.Executables is null)
            {
                return;
            }

            await Task.Run(() =>
            {
                string[] executablesArray = configuration.Executables.Split(',');

                for (int i = 0; i < executablesArray.Length; i++)
                {
                    FileInfo fileInfo = new(Path.Combine(configuration.ApplicationBaseDirectory ?? "", executablesArray[i]));
                    
                    List<Process> processesList = [.. Process.GetProcessesByName(executablesArray[i])];
                    processesList.AddRange([.. Process.GetProcessesByName(executablesArray[i].Replace(".exe", ""))]);

                    LogWritter.Log($"Found '{processesList.Count}' processes with the name '{executablesArray[i]}");

                    foreach (Process process in processesList)
                    {
                        if (process.MainModule is null)
                        {
                            LogWritter.Error($"'{executablesArray[i]}' MainModule not found.");
                            continue;
                        }

                        if (process.MainModule.FileName.Equals(fileInfo.FullName, StringComparison.OrdinalIgnoreCase))
                        {
                            LogWritter.Log($"Finalizando processo {process.Id}-{process.ProcessName} localizado em: {fileInfo.FullName}");
                            process.Kill();
                        }
                    }
                }
            });
        }

        private static async Task ApplyUpdate(Configuration configuration)
        {
            await Task.Run(() =>
            {
                foreach (string file in Directory.GetFiles(configuration.ExtractPath))
                {
                    FileInfo fileInfo = new(file);

                    TryIO.Copy(file, Path.Combine(configuration.ApplicationBaseDirectory!, fileInfo.Name));
                }
            });
        }

        private static async Task StartExecutablesPostUpdate(Configuration configuration)
        {
            if (configuration.Executables is null)
            {
                return;
            }

            await Task.Run(() =>
            {
                string[] executablesArray = configuration.Executables.Split(',');

                for (int i = 0; i < executablesArray.Length; i++)
                {
                    ProcessStartInfo startInfo = new(Path.Combine(configuration.ApplicationBaseDirectory ?? "", executablesArray[i]));
                    Process.Start(startInfo);
                }
            });
        }
    }
}
