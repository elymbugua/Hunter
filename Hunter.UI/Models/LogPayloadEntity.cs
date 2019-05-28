using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hunter.UI.Models
{
    public class LogPayloadEntity
    {
        public string ApplicationId { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string LogMessage { get; set; }
        public string LoggingDate { get; set; }
        public string LogLevel { get; set; }
        public string OS { get; set; }
        public string IpAddress { get; set; }
        public string CpuUtilization { get; set; }
        public string MemoryUtilization { get; set; }
        public IDictionary<object, object> Options { get; set; } = new Dictionary<object, object>();
        public object Exception { get; set; }
    }
}