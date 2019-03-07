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


        public static void Start()
        {
            if (rabbitBus == null)
                rabbitBus = RabbitMQConnection;
        }

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
                return ConfigurationManager.AppSettings["RabbitHost"];
            }
        }

        public static string RabbitPort
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitPort"];
            }
        }

        public static string RabbitUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitUsername"];
            }
        }

        public static string RabbitPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitPassword"];
            }
        }

        public static string RabbitVirtualHost
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitVHost"];
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