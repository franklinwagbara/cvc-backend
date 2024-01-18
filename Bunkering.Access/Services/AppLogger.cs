using Microsoft.Extensions.Configuration;
using Serilog;

namespace Bunkering.Access.Services
{
    public class AppLogger
    {
        public IConfiguration Configuration { get; }
        private readonly string loggerDir = "ApiLogs\\";

        public AppLogger(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void LogRequest(string message, bool isError, string directory)
        {
            var options = $"{loggerDir}\\{directory}\\events.log";
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.File(
               options,
                outputTemplate: "{Timestamp:o} [{Level:u3}] ({SourceContext}) {Message}{NewLine}{Exception}",
                fileSizeLimitBytes: 1_000_000,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
            .CreateLogger();

            if (isError)
                Log.Logger.Error(message);
            else
                Log.Logger.Information(message);
        }
    }
}
