using Core.Log;
using Core.Setup;
using Core.Util.Http;
using System.Text.Json;

namespace Core.Download
{
    internal sealed class DownloadInformation
    {
        public static async ValueTask<Information?> DownloadHttpAsync(Configuration configuration)
        {
            Information? information = null;

            try
            {
                HttpClient client = HttpClientProvider.Factory(configuration.LifeTimeTimeSpan, configuration.TimeOutTimeSpan)
                    .GetClient();

                HttpResponseMessage response = await client.GetAsync(configuration.InformationURL);

                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception($"Could not get download information {response.StatusCode} - {response.ReasonPhrase}");
                }

                string content = await response.Content.ReadAsStringAsync();
                information = JsonSerializer.Deserialize<Information>(content);

                if (information is null)
                {
                    throw new Exception($"Could not read download information '{content}'");
                }
            }
            catch (Exception ex)
            {
                LogWritter.Error($"Error download update information: {ex.Message}");
            }

            return information;
        }
    }
}
