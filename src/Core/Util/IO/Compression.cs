using Core.Log;
using Core.Setup;
using System.IO.Compression;

namespace Core.Util.IO
{
    internal sealed class Compression
    {
        public static void Decompress(ECompressionType compressionType, Configuration configuration)
        {
            switch (compressionType)
            {
                case ECompressionType.Zip:
                    Unzip(configuration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(compressionType), compressionType, null);
            }
        }

        private static void Unzip(Configuration configuration)
        {
            LogWritter.Log($"Extracting '{configuration.DownloadedFilePath}' to '{configuration.ExtractPath}'", true);
            try
            {
                ZipFile.ExtractToDirectory(configuration.DownloadedFilePath, configuration.ExtractPath, true);
            }
            catch (Exception ex)
            {
                LogWritter.Error(ex.Message);
            }
        }
    }
}
