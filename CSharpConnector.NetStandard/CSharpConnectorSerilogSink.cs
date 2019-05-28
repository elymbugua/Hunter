using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            var options = new Dictionary<object, object>();

            if (logEvent.Properties != null)
            {
                foreach(var keyVAluePair in logEvent.Properties)
                {
                    options.Add(JsonConvert.SerializeObject(keyVAluePair.Key),
                        JsonConvert.SerializeObject(keyVAluePair.Value));
                }
            }

            string message = logEvent.RenderMessage(_formatProvider);
            
            var logPayload = new LogPayload
            {
                ApplicationId=Settings.ApplicationId,
                LogCategorization= GetHunterLogLevel(logEvent.Level),               
                LogMessage= message,
                LoggingDate= DateTime.Now,
                Options=options,
                Exception=logEvent.Exception
            };

            LogAggregatorService.PostLog(logPayload,null);
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
