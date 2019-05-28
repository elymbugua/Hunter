using Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Csharp.Connector.NetStandard
{
    public static class SinkExtensions
    {
        public static LoggerConfiguration CSharpConnectorSerilogSink(
            this LoggerSinkConfiguration sinkConfig,
            IFormatProvider formatProvider = null)
        {
            return sinkConfig.Sink(new CSharpConnectorSerilogSink(formatProvider));
        }
    }
}
