using System;
using LabApp.Server.Data;
using LabApp.Shared.Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace LabApp.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DefaultLogger.CreateNlog(null);
            var logger = DefaultLogger.CreateNlog(null);
            try
            {
                var host = CreateHostBuilder(args).Build();
                
                using (var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
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
                
               // return 1;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }

            //return 0;
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
                .UseNLog()
        ;
    }
}
