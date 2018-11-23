using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Hunter.UI.Models
{
	public class RabbitMqManager
	{

        static IBus rabbitBus = null;


        public static void Start()
        {
            if (rabbitBus == null)
                rabbitBus = RabbitMQConnection;
        }

        public static IBus RabbitMQConnection
        {
            get
            {
                if (rabbitBus == null)
                {
                    rabbitBus = RabbitHutch.CreateBus($"host={RabbitHost}:{RabbitPort};virtualHost=" +
                   $"{RabbitVirtualHost};username={RabbitUsername};password={RabbitPassword}");

                    StartSubscriptions(rabbitBus);
                }                           

                return rabbitBus;
            }
        }

        private static void StartSubscriptions(IBus rabbitBus)
        {
            for(int i = 0; i < 10; i++)
            {
                rabbitBus.SubscribeAsync<LogPayload>("logs_subscription", (log) =>
                   new LogsProcessor().HandleLogPayload(log).
                   ContinueWith(task =>
                   {
                        //Log internally to the log aggregator itself
                   }));
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