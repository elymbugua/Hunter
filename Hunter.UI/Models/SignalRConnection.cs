using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Hunter.UI.Models
{
	public class SignalRConnection :PersistentConnection
	{
        private static ConcurrentBag<string> connections = new ConcurrentBag<string>();
        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            return base.OnReceived(request, connectionId, data);
        }

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            connections.Add(connectionId);
            return base.OnConnected(request, connectionId);
        }

        protected override Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {            
            connections.TryTake(out connectionId);
            return base.OnDisconnected(request, connectionId, stopCalled);            
        }

        public static IEnumerable<string> Connections
        {
            get
            {
                return connections;
            }
        }
    }
}