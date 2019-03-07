using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hunter.UI.Models
{
    public class LogCollectionService
    {

        public List<LogPayload> FindLogs(DateTime date)
        {
            return null;
        }

        public List<LogPayload> FindLogs(DateTime date, string Category)
        {
            return null;
        }

        public List<LogPayload> FindLogs(DateTime date,string category,string subCategory)
        {
            return null;
        }

        public List<LogPayload> FindLogs(DateTime date, string category, string subCategory,
            string logLevel)
        {
            return null;
        }

        public LogPayloadEntity GetLogPayloadEntity(LogPayload logPayload)
        {
            if (logPayload == null)
                throw new ArgumentNullException("logPayLoad cannot be null");

            return new LogPayloadEntity
            {
                ApplicationId= logPayload.ApplicationId,
                Category= logPayload.Category,
                Subcategory=logPayload.Subcategory,
                LoggingDate= logPayload.LoggingDate.ToString("dd-MM-yyyy HH:mm"),
                LogLevel=GetLogLevel(logPayload.LogCategorization).ToUpper(),
                LogMessage= logPayload.LogMessage
            };
        }

        private string GetLogLevel(LogConstants logLevel)
        {
            switch (logLevel)
            {
                case LogConstants.Error:
                    return "error";
                case LogConstants.Info:
                    return "info";
                case LogConstants.Critical:
                    return "critical";
                case LogConstants.Warning:
                    return "warning";
                default: return "unknown";
            }
        }
    }
}