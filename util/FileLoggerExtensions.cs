using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace JavaHateBE.Util
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, IConfiguration configuration)
        {
            var path = configuration["Path"];
            builder.AddProvider(new FileLoggerProvider(path));
            return builder;
        }
    }
}