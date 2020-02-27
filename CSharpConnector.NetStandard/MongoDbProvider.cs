using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Hunter.Connector
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

                var mongoConnectionString = Settings.MongoConnectionString;

                var serverUrls = mongoConnectionString.Split(';');

                List<MongoServerAddress> nodes = new List<MongoServerAddress>();

                foreach (var url in serverUrls)
                {
                    nodes.Add(new MongoServerAddress(url));
                }

                mongoClient = new MongoClient(new MongoClientSettings
                {
                    ConnectTimeout = TimeSpan.FromSeconds(5),
                    Servers = nodes
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

    }
}