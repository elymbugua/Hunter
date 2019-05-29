using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hunter.UI.Models;

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

            if (logSearch.StartDate != null && logSearch.EndDate != null &&
                !string.IsNullOrWhiteSpace(logSearch.Category) && !string.IsNullOrWhiteSpace(logSearch.Subcategory)
                && !string.IsNullOrWhiteSpace(logSearch.ApplicationId))
            {
                var searchResults = logsearchService.FilterLogs(logsearchService.ConstructQueryFilters(
                    logSearch.StartDate, logSearch.EndDate, logSearch.Category,
                    logSearch.Subcategory, LogConstants.All, logSearch.Pattern));

                return searchResults;
            }
            else if (logSearch.StartDate != null && logSearch.EndDate != null &&
                !string.IsNullOrWhiteSpace(logSearch.ApplicationId))
            {
                var results = logsearchService.FindLogs(logSearch.StartDate, logSearch.EndDate, logSearch.ApplicationId);
                return results;
            }
            else if (logSearch.StartDate != null && !string.IsNullOrWhiteSpace(logSearch.Category)
                && !string.IsNullOrWhiteSpace(logSearch.Subcategory))
            {
                var results = logsearchService.FindLogs((DateTime)logSearch.StartDate, logSearch.Category, logSearch.Subcategory);
                return results;
            }
            else if (logSearch.StartDate != null && !string.IsNullOrWhiteSpace(logSearch.Category) &&
                !string.IsNullOrWhiteSpace(logSearch.Subcategory))
            {
                var results = logsearchService.FindLogs((DateTime)logSearch.StartDate,
                    logSearch.Category, logSearch.Subcategory, LogConstants.All);
                return results;
            }
            else if (logSearch.StartDate != null && logSearch.EndDate != null)
            {
                var filters = logsearchService.ConstructQueryFilters(logSearch.StartDate, logSearch.EndDate, logSearch.Category,
                   logSearch.Subcategory, LogConstants.All, logSearch.Pattern);

                var results = logsearchService.FilterLogs(filters);
                return results;
            }
            else if (logSearch.StartDate != null)
            {
                var results = logsearchService.FindLogs((DateTime)logSearch.StartDate);
                return results;
            }
            else
            {
                var filters = logsearchService.ConstructQueryFilters(logSearch.StartDate, logSearch.EndDate, logSearch.Category,
                    logSearch.Subcategory, LogConstants.All, logSearch.Pattern);

                var results = logsearchService.FilterLogs(filters);
                return results;
            }
        }
    }
}
