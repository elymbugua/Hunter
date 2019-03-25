using EasyNetQ;
using EasyNetQ.Topology;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.UI.Models
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
                    rabbitBus = RabbitHutch.CreateBus($"host={RabbitHost}:{RabbitPort};username={RabbitUsername};password={RabbitPassword}").Advanced;

                    StartSubscriptions(rabbitBus);
                }                           

                return rabbitBus;
            }
        }

        private static void StartSubscriptions(IAdvancedBus rabbitBus)
        {
            var queue = rabbitBus.QueueDeclare("applogs");

            rabbitBus.Consume(queue, (body, properties, info) => Task.Factory.StartNew(() =>
            {
                var message = Encoding.UTF8.GetString(body);
                var log = JsonConvert.DeserializeObject<LogPayload>(message);

                //Save and stream the message                  

                if (log != null)
                {
                    Debug.WriteLine($"[{log.LoggingDate.ToString("dd-MM-yy HH:mm")}] " +
                        $"AppId:[{log.ApplicationId}] [{log.LogCategorization}] {log.LogMessage}");

                    var logPayloadEntity = new LogCollectionService().GetLogPayloadEntity(log);

                    new RealtimeHub().SendLogMessage(logPayloadEntity);
                    MongoDbProvider.GetHunterLogsCollection().InsertOne(logPayloadEntity);
                }
            }));
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