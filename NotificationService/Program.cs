using System;
using LabApp.Shared.Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using NotificationService.DAL;

namespace NotificationService
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var logger = DefaultLogger.CreateNlog(null);
            try
            {
                var host = CreateHostBuilder(args).Build();
                
                using (var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                using (var context = serviceScope.ServiceProvider.GetService<NotificationDbContext>())
                {
                    logger.Info("Migrations started ...");
                    context.Database.Migrate();
                    logger.Info("Migrations finished.");
                }
                
                host.Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                return 0;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }

            return 1;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(x =>
                        x.Configure<HostOptions>(y => y.ShutdownTimeout = TimeSpan.FromSeconds(30)));
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
