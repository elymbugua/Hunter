using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Connector
{
    public class LogAggregatorService
    {
        private readonly static Logger localLogger = null;

        private static LogPayload GetLogPayload(string message, LogConstants logCategory, string appId = null)
        {
            var logPayload = new LogPayload
            {
                ApplicationId = appId ?? "",
                LogCategorization = logCategory,
                LoggingDate = DateTime.Now,
                LogMessage = message,
                LoggingSource = LogSource.Custom,
                Runtime=".Net"
            };

            return logPayload;
        }

        public static void PostError(string message, string appId = null)
        {
            if (message == null)
                throw new ArgumentException("Log message cannot be null");            

            PostLog(GetLogPayload(message, LogConstants.Error, appId));
        }

        public static void PostInfo(string message, string appId = null)
        {
            if (message == null)
                throw new ArgumentException("Log message cannot be null");

            PostLog(GetLogPayload(message, LogConstants.Info, appId));
        }

        public static void PostLog(LogPayload payLoad, string absoulteLogFileName = null)
        {
            if (payLoad == null)
                throw new ArgumentNullException("payload cannot be null");

            try
            {                
                MongoDbProvider.GetHunterLogsCollection().InsertOne(payLoad);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occured publishing log");
                if (absoulteLogFileName != null)
                {
                    var localLogger = GetLocalLogger(absoulteLogFileName);
                    localLogger.Error(ex.ToString());
                    LogLocally(payLoad, localLogger);
                }                   
            }

        }

        private static void LogLocally(LogPayload logPayload, Logger logger)
        {
            switch (logPayload.LogCategorization)
            {
                case LogConstants.Critical:
                    logger.Fatal("{[AppId]} {Date} {Log}", logPayload.ApplicationId, logPayload.LoggingDate, logPayload.LogMessage);
                    break;
                case LogConstants.Error:
                    logger.Error("{[AppId]} {Date} {Log}", logPayload.ApplicationId, logPayload.LoggingDate, logPayload.LogMessage);
                    break;
                case LogConstants.Info:
                    logger.Information("{[AppId]} {Date} {Log}", logPayload.ApplicationId, logPayload.LoggingDate, logPayload.LogMessage);
                    break;
                case LogConstants.Warning:
                    logger.Warning("{[AppId]} {Date} {Log}", logPayload.ApplicationId, logPayload.LoggingDate, logPayload.LogMessage);
                    break;
            }
        }

        public static void SetupUnhandledExceptionsHandler()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.CSharpConnectorSerilogSink()
                .CreateLogger();
        }

        public static Logger GetLocalLogger(string absoluteLogFileName)
        {
            if (localLogger != null)
                return localLogger;

            var logger = new LoggerConfiguration()
                .WriteTo.File(absoluteLogFileName, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            return logger;
        }

        public static string GetLogLevel(LogConstants constant)
        {
            switch (constant)
            {
                case LogConstants.Critical:
                    return "critical";
                case LogConstants.Error:
                    return "error";
                case LogConstants.Info:
                    return "info";
                case LogConstants.Warning:
                    return "warning";
            }

            return "unknown";
        }
    }
}
