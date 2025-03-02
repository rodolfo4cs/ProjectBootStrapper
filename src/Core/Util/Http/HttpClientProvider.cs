namespace Core.Util.Http
{
    internal static class HttpClientProvider
    {
        private static HttpClientFactory? _factory;

        public static HttpClientFactory? Get()
        {
            return _factory;
        }

        public static HttpClientFactory Factory(TimeSpan lifetime, TimeSpan timeout)
        {
            _factory ??= new(lifetime, timeout);

            return _factory;
        }

        public static HttpClientFactory Factory(TimeSpan lifetime, TimeSpan timeout, HttpClientHandler httpClientHandler)
        {
            _factory ??= new(lifetime, timeout, httpClientHandler);

            return _factory;
        }
    }
}
