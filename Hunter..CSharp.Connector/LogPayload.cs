using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.CSharp.Connector
{
    public enum LogConstants
    {
        Info,
        Error,
        Warning,
        Critical
    }
    public class LogPayload
    {
        public string ApplicationId { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public LogConstants LogCategorization { get; set; }
        public DateTime LoggingDate { get; set; }
        public string LogMessage { get; set; }            
    }
}
