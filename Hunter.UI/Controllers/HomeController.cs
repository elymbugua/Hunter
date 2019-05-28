using Hunter.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hunter.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var logs = new LogCollectionService().GetLatestLogs();

            LogsTimer.siteLoaded = true;

            return View(logs);
        }

        public ActionResult Logs()
        {
            var logs = new LogCollectionService().GetNewLogs().
                Result.OrderBy(log=>log.LoggingDate).Take(100).ToList();

            LogsTimer.siteLoaded = true;

            return View(logs);
        }

        public JsonResult SearchLogs(LogSearch logSearch)
        {
            if (logSearch == null)
                return Json(new List<LogPayloadEntity>(), JsonRequestBehavior.AllowGet);

            var logsearchService = new LogCollectionService();

            if(logSearch.StartDate!=null && logSearch.EndDate!=null && 
                !string.IsNullOrWhiteSpace(logSearch.Category) && !string.IsNullOrWhiteSpace(logSearch.Subcategory)
                && !string.IsNullOrWhiteSpace(logSearch.ApplicationId))
            {
                var searchResults= logsearchService.FilterLogs(logsearchService.ConstructQueryFilters(
                    logSearch.StartDate, logSearch.EndDate, logSearch.Category,
                    logSearch.Subcategory, LogConstants.All, logSearch.Pattern));

                return Json(searchResults, JsonRequestBehavior.AllowGet);                
            }
            else if(logSearch.StartDate!=null && logSearch.EndDate!=null &&
                !string.IsNullOrWhiteSpace(logSearch.ApplicationId))
            {
                var results=logsearchService.FindLogs(logSearch.StartDate, logSearch.EndDate, logSearch.ApplicationId);
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            else if(logSearch.StartDate!=null && !string.IsNullOrWhiteSpace(logSearch.Category)
                && !string.IsNullOrWhiteSpace(logSearch.Subcategory))
            {
                var results = logsearchService.FindLogs(logSearch.StartDate, logSearch.Category, logSearch.Subcategory);                
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            else if(logSearch.StartDate!=null && !string.IsNullOrWhiteSpace(logSearch.Category) &&
                !string.IsNullOrWhiteSpace(logSearch.Subcategory)){
                var results = logsearchService.FindLogs(logSearch.StartDate,
                    logSearch.Category, logSearch.Subcategory, LogConstants.All);
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            else if(logSearch.StartDate!=null && logSearch.EndDate != null)
            {
                var filters = logsearchService.ConstructQueryFilters(logSearch.StartDate, logSearch.EndDate, logSearch.Category,
                   logSearch.Subcategory, LogConstants.All, logSearch.Pattern);

                var results = logsearchService.FilterLogs(filters);
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            else if (logSearch.StartDate != null)
            {
                var results = logsearchService.FindLogs(logSearch.StartDate);
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var filters = logsearchService.ConstructQueryFilters(logSearch.StartDate, logSearch.EndDate, logSearch.Category,
                    logSearch.Subcategory, LogConstants.All, logSearch.Pattern);

                var results = logsearchService.FilterLogs(filters);
                return Json(results, JsonRequestBehavior.AllowGet);
            }

            return Json(new List<LogPayloadEntity>(), JsonRequestBehavior.AllowGet);
        }
    }
}
