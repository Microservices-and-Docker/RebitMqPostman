using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace RabbitMqPostman
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                NLogBuilder.ConfigureNLog("NLog.config")
                           .GetCurrentClassLogger();

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
               .UseNLog();
    }
}
