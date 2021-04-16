using Microsoft.Extensions.DependencyInjection;
using RebitMqPostman.Common.Models;

namespace RebitMqPostman.Configuration.Services
{
    public static class NLogServiceExtension
    {
        public static void AddNLogConfiguration(this IServiceCollection services, AppSettings appConfiguration)
        {
            NLog.GlobalDiagnosticsContext.Set("LogDirectory", appConfiguration.LogDirectory);
            NLog.Common.InternalLogger.LogFile = $@"{appConfiguration.LogDirectory}\nlog-internal.log";
        }
    }
}
