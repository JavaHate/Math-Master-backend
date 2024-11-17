using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace JavaHateBE.Util
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _path;

        public FileLoggerProvider(string path)
        {
            _path = path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_path);
        }

        public void Dispose()
        {
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string _path;
        private static readonly object _lock = new object();

        public FileLogger(string path)
        {
            _path = path;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);
            if (!string.IsNullOrEmpty(message))
            {
                lock (_lock)
                {
                    File.AppendAllText(_path, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} [{logLevel}] {message}{Environment.NewLine}");
                }
            }
        }
    }
}