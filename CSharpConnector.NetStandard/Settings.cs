using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Connector
{
    public class HunterSettings
    {
        public static string ApplicationId { get; set; } = string.Empty; 
        public static string MongoConnectionString { get; set; }
    }
}
