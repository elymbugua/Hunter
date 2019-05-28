using System;
using System.Threading.Tasks;
using Hunter.UI.Models;
using Microsoft.Owin;
using MongoDB.Driver;
using Owin;

[assembly: OwinStartup(typeof(Hunter.UI.Startup))]

namespace Hunter.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR<SignalRConnection>("/realtimelogs");
            var dateIndex = Builders<LogPayload>.IndexKeys.Descending(pl => pl.LoggingDate);
            var appIndex = Builders<LogPayload>.IndexKeys.Ascending(pl => pl.ApplicationId);
            var categoryIndex = Builders<LogPayload>.IndexKeys.Ascending(pl => pl.Category);
            var subCategoryIndex = Builders<LogPayload>.IndexKeys.Ascending(pl => pl.Subcategory);
            var logCategorizationIndex = Builders<LogPayload>.IndexKeys.Ascending(pl => pl.LogCategorization);

            MongoDbProvider.GetHunterLogsCollection().Indexes.CreateOne(dateIndex);
            MongoDbProvider.GetHunterLogsCollection().Indexes.CreateOne(appIndex);
            MongoDbProvider.GetHunterLogsCollection().Indexes.CreateOne(categoryIndex);
            MongoDbProvider.GetHunterLogsCollection().Indexes.CreateOne(subCategoryIndex);
            MongoDbProvider.GetHunterLogsCollection().Indexes.CreateOne(logCategorizationIndex);
        }
    }
}
