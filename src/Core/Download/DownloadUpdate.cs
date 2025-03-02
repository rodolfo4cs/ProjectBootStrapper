using Core.Log;
using Core.Setup;
using Core.Util.IO;
using System.Diagnostics;

namespace Core.Download
{
    internal static class DownloadUpdate
    {
        public static async ValueTask DownloadHttpAsync(Configuration configuration, Information information)
        {
            long existingLastReadPosition = 0;

            if (configuration.Overwrite.GetValueOrDefault(true))
            {
                TryIO.Delete(configuration.DownloadedFilePath);
            }

            if (File.Exists(configuration.DownloadedFilePath))
            {
                existingLastReadPosition = new FileInfo(configuration.DownloadedFilePath).Length;
                LogWritter.Log($"File already exists with size of '{existingLastReadPosition}' bytes.");
            }

            HttpResponseMessage? response = await DownloadHelper.GetHttpResponseAsync(information.DownloadUrl, configuration.LifeTimeTimeSpan, configuration.TimeOutTimeSpan, existingLastReadPosition).ConfigureAwait(false);

            if (response is null)
            {
                LogWritter.Log($"Response is null. Aborting!");
                return;
            }

            long fileSize = response.Content.Headers.ContentLength.GetValueOrDefault(0);

            if (fileSize == 0)
            {
                LogWritter.Log($"Retrieved filesize to be downloaded returned 0. Aborting!");
                return;
            }

            if (existingLastReadPosition > 0 && existingLastReadPosition == fileSize && configuration.Overwrite.GetValueOrDefault(true) == false)
            {
                LogWritter.Log($"Existing filesize of '{existingLastReadPosition}' is equal to be downloaded filesize of '{fileSize}' and overwrite is set to false. Aborting!");
                return;
            }

            await DownloadAsync(configuration, response, existingLastReadPosition, fileSize).ConfigureAwait(false);
        }

        private static async Task DownloadAsync(Configuration configuration, HttpResponseMessage response, long existingLastReadPosition, long fileSize)
        {
            int currentPercentage = -1;
            long startTime = Stopwatch.GetTimestamp();

            try
            {
                Directory.CreateDirectory(configuration.ApplicationBaseDirectory!);
                Directory.CreateDirectory(configuration.DownloadPath!);

                await using FileStream outputStream = new(configuration.DownloadedFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                outputStream.Position = existingLastReadPosition;
                byte[] bufferArray = new byte[configuration.DefaultBufferSize!.Value];

                await using Stream inputStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                int bytesRead;
                long currentPosition = existingLastReadPosition;

                do
                {
                    bytesRead = await inputStream.ReadAsync(bufferArray.AsMemory(0, bufferArray.Length)).ConfigureAwait(false);

                    await outputStream.WriteAsync(bufferArray.AsMemory(0, bytesRead)).ConfigureAwait(false);
                    currentPosition += bytesRead;

                    int newPercentage = (int)((currentPosition * 100) / fileSize);
                    if (newPercentage > currentPercentage && newPercentage % 10 == 0)
                    {
                        currentPercentage = newPercentage;
                        LogWritter.Log($"Download progress: {currentPercentage}%.", true);
                    }
                    LogWritter.Log($"Download progress: {currentPosition} bytes downloaded.");

                    if (File.Exists("stop_download.test"))
                    {
                        LogWritter.Log($"Download stopped by test.");
                        break;
                    }
                }
                while (bytesRead > 0);

                LogWritter.Log($"Download completed. Time {Stopwatch.GetElapsedTime(startTime)}. Total bytes downloaded: {currentPosition - fileSize}");
            }
            catch (Exception ex)
            {
                LogWritter.Error($"Error downloading update: {ex.Message}");
            }
            finally
            {
                response.Dispose();
            }
        }
    }
}
