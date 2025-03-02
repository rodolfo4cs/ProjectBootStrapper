namespace Core.Util.Http
{
    /// <summary>
    /// Do not dispose the HttpClient, it is managed by the factory.
    /// </summary>
    internal sealed class HttpClientFactory
    {
        private readonly TimeSpan _lifetime;
        private readonly TimeSpan _timeout;
        private readonly HttpClientHandler _httpClientHandler;
        private HttpClient _httpClient;
        private DateTimeOffset _lastRenewal;

        public HttpClientFactory(TimeSpan lifetime, TimeSpan timeout)
        {
            _lifetime = lifetime;
            _timeout = timeout;
            _httpClientHandler = new HttpClientHandler { AllowAutoRedirect = true };
            _httpClient = CreateHttpClient();
            _lastRenewal = DateTimeOffset.Now;
        }

        public HttpClientFactory(TimeSpan lifetime, TimeSpan timeout, HttpClientHandler httpClientHandler)
        {
            _lifetime = lifetime;
            _timeout = timeout;
            _httpClientHandler = httpClientHandler;
            _httpClient = CreateHttpClient();
            _lastRenewal = DateTimeOffset.Now;
        }

        public HttpClient GetClient()
        {
            if (DateTimeOffset.Now - _lastRenewal > _lifetime)
            {
                RenewHttpClient();
            }

            return _httpClient;
        }

        private void RenewHttpClient()
        {
            _httpClient = CreateHttpClient();
            _lastRenewal = DateTimeOffset.Now;
        }

        private HttpClient CreateHttpClient()
        {
            return new HttpClient(_httpClientHandler)
            {
                Timeout = _timeout,
                DefaultRequestHeaders = { { "User-Agent", "MyApp" } }
            };
        }
    }
}
