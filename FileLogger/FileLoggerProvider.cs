
public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _path;

    public FileLoggerProvider(string path)
    {
        _path = path;
    }

    public ILogger CreateLogger(string categoryName)
    {
        var loggerType = typeof(FileLogger<>).MakeGenericType(Type.GetType(categoryName) ?? typeof(object));
        return (ILogger)Activator.CreateInstance(loggerType, _path)!;
    }

    public void Dispose() { }
}