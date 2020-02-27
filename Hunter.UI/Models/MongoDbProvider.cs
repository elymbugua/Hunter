using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Hunter.UI.Models
{
    public class MongoDbProvider
    {
        private static MongoClient mongoClient = null;

        public static MongoClient MongoClient
        {
            get
            {
                if (mongoClient != null)
                    return mongoClient;

                var mongoConnectionString = ConfigurationManager.AppSettings["MongoConnectionString"];                

                var serverUrls = mongoConnectionString.Split(';');
                List<MongoServerAddress> nodes = new List<MongoServerAddress>();

                foreach(var url in serverUrls)
                {
                    var urlParts = url.Split(':');
                    nodes.Add(new MongoServerAddress(urlParts[0],int.Parse(urlParts[1])));
                }

                mongoClient = new MongoClient(new MongoClientSettings
                {
                    ConnectTimeout= TimeSpan.FromSeconds(30),
                    Servers= nodes                    
                });

                return mongoClient;
            }
        }
        
        public static IMongoDatabase GetHunterLogsDatabase()
        {
            return MongoClient.GetDatabase("hunterlogsdb");
        }

        public static IMongoCollection<LogPayload> GetHunterLogsCollection()
        {

            return GetHunterLogsDatabase().GetCollection<LogPayload>("hunterlogs");
        }

        public static IMongoCollection<LatestDateEntity> GetLatestDateCollection()
        {
            return GetHunterLogsDatabase().GetCollection<LatestDateEntity>("latestlogstimestamp");
        }
    }
}