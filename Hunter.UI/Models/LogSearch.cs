using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hunter.UI.Models
{
    public class LogSearch
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string ApplicationId { get; set; }
        public string LogLevel { get; set; }
        //public string Pattern { get; set; }
    }
}