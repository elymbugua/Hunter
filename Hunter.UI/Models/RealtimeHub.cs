using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Timers;
using System.Web;

namespace Hunter.UI.Models
{
    public class RealtimeHub
    {
        static IConnection _persistentConnection = GlobalHost.ConnectionManager.GetConnectionContext<SignalRConnection>().Connection;       
       
        public static void SendMessages(List<LogPayloadEntity> messages)
        {
            foreach (var message in messages)
            {
                SendLogMessage(message);
            }

        }

        public static void SendLogMessage(LogPayloadEntity logPayload)
        {
            foreach (var connection in SignalRConnection.Connections)
            {
                _persistentConnection.Send(connection, JsonConvert.SerializeObject(logPayload));
            }
        }

        

        
    }
}