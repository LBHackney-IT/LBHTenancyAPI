using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace LBHTenancyAPI.Infrastructure.Logging
{
    public class SentryLoggerProvider : ILoggerProvider
    {
        private readonly string _url;
        private readonly ConcurrentDictionary<string, SentryLogger> _loggers = new ConcurrentDictionary<string, SentryLogger>();
        public SentryLoggerProvider(string url)
        {
            _url = url;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new SentryLogger(name, _url));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}