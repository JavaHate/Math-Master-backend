namespace JavaHateBE.FileLogger
{
    public class NoOpLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new NoOpLogger();

        public void Dispose() { }

        private class NoOpLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

            public bool IsEnabled(LogLevel logLevel) => false;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }

            private class NullScope : IDisposable
            {
                public static NullScope Instance { get; } = new NullScope();

                public void Dispose() { }
            }
        }
    }
}