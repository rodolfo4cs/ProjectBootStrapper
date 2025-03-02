using Core.Log;
using Core.Util.Http;
using System.Net.Http.Headers;

namespace Core.Download
{
    internal static class DownloadHelper
    {
        public static async Task<HttpResponseMessage?> GetHttpResponseAsync(string url, TimeSpan lifeTimeTimeSpan, TimeSpan timeOutTimeSpan, long existingLastReadPosition = 0)
        {
            try
            {
                HttpClient client = HttpClientProvider.Factory(lifeTimeTimeSpan, timeOutTimeSpan)
                    .GetClient();

                client.DefaultRequestHeaders.Range = new RangeHeaderValue(existingLastReadPosition, null);

                HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Status code: {response.StatusCode} - {response.ReasonPhrase}");
                }

                LogWritter.Log($"Successfully retrieved response from url '{url}'.");

                return response;
            }
            catch (InvalidOperationException ioex)
            {
                LogWritter.Error($"Invalid operation while getting response from url '{url}'. Error: {ioex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                LogWritter.Error($"Error getting response from url '{url}'. Error: {ex.Message}");
                return null;
            }
        }
    }
}
