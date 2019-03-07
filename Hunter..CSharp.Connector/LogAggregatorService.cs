using EasyNetQ;
using EasyNetQ.Topology;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.CSharp.Connector
{
    public class LogAggregatorService
    {
        static IAdvancedBus messageBus = null;
        private readonly static Logger localLogger = null;

        public static IAdvancedBus MessageBus
        {
            get
            {
                if (messageBus == null)
                {
                    messageBus = RabbitMqManager.RabbitMQConnection;
                }

                return messageBus;
            }           
        }

        private static LogPayload GetLogPayload(string message,LogConstants logCategory, string appId = null)
        {
            var logPayload = new LogPayload
            {
                ApplicationId = appId ?? "",
                LogCategorization = logCategory,
                LoggingDate = DateTime.Now,
                LogMessage = message
            };

            return logPayload;
        }

        public static void PostError(string message, string appId=null)
        {
            if (message == null)
                throw new ArgumentException("Log message cannot be null");

            PostLog(GetLogPayload(message,LogConstants.Error, appId));
        }

        public static void PostInfo(string message, string appId = null)
        {
            if (message == null)
                throw new ArgumentException("Log message cannot be null");

            PostLog(GetLogPayload(message, LogConstants.Info, appId));
        }

        public static void PostLog(LogPayload payLoad, string absoulteLogFileName=null)
        {
            if (payLoad == null)
                throw new ArgumentNullException("payload cannot be null");
            
            string queueName = "applogs";
            var logsExchenge=MessageBus.ExchangeDeclare("applogs-exchange", ExchangeType.Direct);
            var logsQueue= MessageBus.QueueDeclare(queueName);
            MessageBus.Bind(logsExchenge, logsQueue,"A.B");
            var message = new Message<LogPayload>(payLoad);

            try
            {
                MessageBus.Publish(Exchange.GetDefault(), queueName, false, message);
            }
            catch(Exception ex)
            {
                var localLogger = GetLocalLogger(absoulteLogFileName);
                localLogger.Error(ex.ToString());

                if (absoulteLogFileName != null)
                    LogLocally(payLoad,localLogger);
            }
            
        }

        private static void LogLocally(LogPayload logPayload,Logger logger)
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
    }
}
