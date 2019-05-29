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

        
    }
}
