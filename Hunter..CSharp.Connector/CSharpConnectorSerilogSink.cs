using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.CSharp.Connector
{
    public class CSharpConnectorSerilogSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;
        public CSharpConnectorSerilogSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            var logPayload = new LogPayload
            {
                ApplicationId="Serilog",
                LogCategorization= GetHunterLogLevel(logEvent.Level),
                LogMessage= JsonConvert.SerializeObject(logEvent.MessageTemplate),
                LoggingDate= DateTime.Now
            };

            LogAggregatorService.PostLog(logPayload);
        }

        private LogConstants GetHunterLogLevel(LogEventLevel logEventLevel)
        {
            switch (logEventLevel)
            {
                case LogEventLevel.Error:
                    return LogConstants.Error;
                case LogEventLevel.Information:
                    return LogConstants.Info;
                case LogEventLevel.Warning:
                    return LogConstants.Warning;
                default: return LogConstants.Info;
            }
        }
    }

    
}
