using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hunter.CSharp.Connector
{
    public class LogPayloadEntity
    {
        public string ApplicationId { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string LogMessage { get; set; }
        public DateTime LoggingDate { get; set; }
        public string LogLevel { get; set; }
        public Dictionary<string, object> Extras { get; set; } = new Dictionary<string, object>();
    }
}