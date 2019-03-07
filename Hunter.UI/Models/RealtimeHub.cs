using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hunter.UI.Models
{
    public class RealtimeHub
    {
        IConnection _persistentConnection = GlobalHost.ConnectionManager.GetConnectionContext<SignalRConnection>().Connection;
        public void SendMessages(List<LogPayloadEntity> messages)
        {
            foreach (var message in messages)
            {
                SendLogMessage(message);
            }

        }

        public void SendLogMessage(LogPayloadEntity logPayload)
        {
            foreach (var connection in SignalRConnection.Connections)
            {
                _persistentConnection.Send(connection, JsonConvert.SerializeObject(logPayload));
            }
        }
    }
}