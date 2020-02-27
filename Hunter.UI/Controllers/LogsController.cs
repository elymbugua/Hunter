using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hunter.UI.Models;
using MongoDB.Driver;

namespace Hunter.UI.Controllers
{
    public class LogsController : ApiController
    {        
        [HttpGet]
        public List<LogPayloadEntity> SearchLogs([FromUri]LogSearch logSearch)
        {
            if (logSearch == null)
                return new List<LogPayloadEntity>();

            var logsearchService = new LogCollectionService();
            var results = logsearchService.FindLogs(logSearch.StartDate, logSearch.EndDate, logSearch.ApplicationId);
            return results;
        }
       
    }
}
