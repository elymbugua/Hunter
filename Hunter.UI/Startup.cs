using System;
using System.Threading.Tasks;
using Hunter.UI.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Hunter.UI.Startup))]

namespace Hunter.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR<SignalRConnection>("/realtimelogs");
        }
    }
}
