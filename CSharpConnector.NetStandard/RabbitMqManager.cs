using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Hunter.CSharp.Connector
{
	public class RabbitMqManager
	{

        static IAdvancedBus rabbitBus = null;        
       
        public static IAdvancedBus RabbitMQConnection
        {
            get
            {
                if (rabbitBus == null)
                {
                    rabbitBus = RabbitHutch.CreateBus($"host={RabbitHost}:" +
                        $"{RabbitPort};username={RabbitUsername};password={RabbitPassword}").Advanced;                    
                }                           

                return rabbitBus;
            }
        }      

        public static string RabbitHost
        {
            get
            {
                if (Settings.RabbitHost == null)
                    throw new ArgumentNullException("RabbitHost cannot be null");

                return Settings.RabbitHost;
            }
        }

        public static string RabbitPort
        {
            get
            {
                if (Settings.RabbitPort == null)
                    throw new ArgumentNullException("RabbitPort cannot be null");

                return Settings.RabbitPort;
            }
        }

        public static string RabbitUsername
        {
            get
            {
                if (Settings.RabbitUserName == null)
                    throw new ArgumentNullException("RabbitUserName cannot be null");

                return Settings.RabbitUserName;
            }
        }

        public static string RabbitPassword
        {
            get
            {
                if (Settings.RabbitPassword == null)
                    throw new ArgumentNullException("RabbitPassword cannot be null");

                return Settings.RabbitPassword;
            }
        }

        public static string RabbitVirtualHost
        {
            get
            {
                if (Settings.RabbitVirtualHost == null)
                    throw new ArgumentNullException("RabbitVirtualHost cannot be null");

                return Settings.RabbitVirtualHost;
            }
        }
        

        public static void DisposeBus()
        {
            if (rabbitBus != null)
            {
                rabbitBus.Dispose();
                rabbitBus = null;
            }
               
        }
        
	}
}