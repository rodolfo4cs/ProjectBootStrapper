using Core.Log;

namespace Core.Util.IO
{
    internal static class TryIO
    {
        public static bool Delete(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                LogWritter.Error($"Error deleting file: {path}. Error: {ex.Message}");
            }

            return false;
        }

        public static void Copy(string source, string destination)
        {
            try
            {
                File.Copy(source, destination, true);
            }
            catch (Exception ex)
            {
                LogWritter.Error($"Error copying file: {source} to {destination}. Error: {ex.Message}");
            }
        }
    }
}
