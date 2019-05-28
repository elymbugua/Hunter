using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Hosting;

namespace Hunter.UI.Models
{
    public class LogsTimer : IRegisteredObject
    {
        public static bool siteLoaded = false;
        static Timer logsTimer;
        static object timerLock = new object();

        public LogsTimer()
        {
            int timerInterval = 0;

            if (!int.TryParse(ConfigurationManager.AppSettings["TimerInterval"], out timerInterval))
            {
                timerInterval = 2000; //in milliseconds
            }

            logsTimer = new Timer(timerInterval);
            logsTimer.AutoReset = true;
            logsTimer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            logsTimer.Start();
        }

        public static void TimerElapsed(object sender, ElapsedEventArgs args)
        {
            lock (timerLock)
            {
                if (!siteLoaded) return;

                var newLogs = new LogCollectionService().GetNewLogs().Result;
                Log.Information("Checking new Logs");

                if (newLogs != null)
                {
                    RealtimeHub.SendMessages(newLogs);
                }
            }
            
        }

        public void Stop(bool immediate)
        {
            logsTimer.Dispose();
            HostingEnvironment.UnregisterObject(this);
        }
    }
}