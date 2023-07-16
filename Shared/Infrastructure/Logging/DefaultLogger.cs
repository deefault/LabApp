using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;

namespace LabApp.Shared.Infrastructure.Logging
{
    public class DefaultLogger
    {
        public static Logger CreateNlog(IConfiguration configuration)
        {
            
            
            
            var logconsole = new NLog.Targets.ColoredConsoleTarget("logconsole");
            var config = new LoggingConfiguration()
            {
                
            };
            config.AddRule(LogLevel.Off, loggerNamePattern:"Microsoft.AspNetCore.SignalR", final :true, target: logconsole, maxLevel: LogLevel.Debug);
            config.AddRule(LogLevel.Off, loggerNamePattern:"Microsoft.AspNetCore.Http.Connections", final :true, target: logconsole, maxLevel: LogLevel.Debug);
            config.AddRule(LogLevel.Off, loggerNamePattern:"Microsoft.*", final :true, target: logconsole, maxLevel: LogLevel.Info);
            config.AddRule(LogLevel.Off, LogLevel.Trace, logconsole, loggerNamePattern: "*");
            config.AddRule(LogLevel.Off, LogLevel.Trace, logconsole, loggerNamePattern: "*");
            //NLog.Web.NLogBuilder.ConfigureNLog(config);
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Log(LogLevel.Debug, "init logger");

            return logger;
        }
    }
}