using Microsoft.Extensions.Logging;
using System;
using System.IO;

public class FileLogger<T> : ILogger<T>
{
    private readonly string _path;

    public FileLogger(string path)
    {
        _path = path;
    }

    public IDisposable BeginScope<TState>(TState state) => null!;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = $"{DateTime.Now} [{logLevel}] {formatter(state, exception)}{Environment.NewLine}";
        File.AppendAllText(_path, message);
    }
}