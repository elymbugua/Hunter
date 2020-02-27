using Innova.Commons;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Hunter.UI.Models
{
    public class LogCollectionService
    {

        public List<LogPayloadEntity> FindLogs(DateTime loggingDate)
        {           
            var builder = Builders<LogPayload>.Filter;
            var date = loggingDate.Date;
            var nextDay = loggingDate.Date.AddDays(1);                   
            var filter = builder.Gte(pl => pl.LoggingDate,new BsonDateTime(date)) & builder.Lt(pl => pl.LoggingDate, new BsonDateTime(nextDay));           
            var results = MongoDbProvider.GetHunterLogsCollection().Find(filter).ToList();
            var logs = new List<LogPayloadEntity>();

            results.ForEach(log =>
            {
                logs.Add(GetLogPayloadEntity(log));
            });

            return logs;
        }

        public List<LogPayloadEntity> FindLogs(DateTime date, string Category)
        {
            var builder = Builders<LogPayload>.Filter;
            var nextDay = date.Date.AddDays(1);
            var filter = builder.Gte(pl => pl.LoggingDate, date) & builder.Lt(pl => pl.LoggingDate, nextDay) &
                builder.Eq(pl => pl.Category, Category);

            var results = MongoDbProvider.GetHunterLogsCollection().Find(filter);
            var logs = new List<LogPayloadEntity>();

            results.ForEachAsync(log =>
            {
                logs.Add(GetLogPayloadEntity(log));
            });

            return logs;
        }

        public List<LogPayloadEntity> FindLogs(DateTime date,string category,string subCategory)
        {
            var builder = Builders<LogPayload>.Filter;
            var nextDay = date.Date.AddDays(1);
            var filter= builder.Gte(pl => pl.LoggingDate, date) & builder.Lt(pl => pl.LoggingDate, nextDay) &
                builder.Eq(pl => pl.Category, category) & builder.Eq(pl=>pl.Subcategory,subCategory);

            var results = MongoDbProvider.GetHunterLogsCollection().Find(filter);
            var logs = new List<LogPayloadEntity>();

            results.ForEachAsync(log =>
            {
                logs.Add(GetLogPayloadEntity(log));
            });

            return logs;
        }

        public List<LogPayloadEntity> FindLogs(DateTime date, string category, string subCategory,
            LogConstants logLevel)
        {
            var builder = Builders<LogPayload>.Filter;
            var nextDay = date.Date.AddDays(1);
            var filter = builder.Gte(pl => pl.LoggingDate, date) & builder.Lt(pl => pl.LoggingDate, nextDay) &
                builder.Eq(pl => pl.Category, category) & builder.Eq(pl => pl.Subcategory, subCategory) 
                & builder.Eq(pl=>pl.LogCategorization, logLevel);

            var results = MongoDbProvider.GetHunterLogsCollection().Find(filter);
            var logs = new List<LogPayloadEntity>();

            results.ForEachAsync(log =>
            {
                logs.Add(GetLogPayloadEntity(log));
            });

            return logs;
        }

        public List<LogPayloadEntity> GetLogs(string applicationId)
        {            
            var results = MongoDbProvider.GetHunterLogsCollection().Find(lp=>lp.ApplicationId==applicationId);
            var logs = new List<LogPayloadEntity>();

            results.ForEachAsync(log =>
            {
                logs.Add(GetLogPayloadEntity(log));
            });

            return logs;
        }

        public List<LogPayloadEntity> FindLogs(DateTime? startDate, DateTime? endingDate, string applicationId)
        {
            var builder = Builders<LogPayload>.Filter;
            var logs = new List<LogPayloadEntity>();            

            var filterBuilder = new StringBuilder();

            if(startDate!=null && endingDate != null && !string.IsNullOrWhiteSpace(applicationId))
            {
                var date1 = DateUtilities.FormatDateToIsoFormat(((DateTime)startDate));
                var date2 = DateUtilities.FormatDateToIsoFormat(((DateTime)endingDate).AddDays(1));

                filterBuilder.Append("{ LoggingDate:{$gte:ISODate('").Append(date1).
                    Append("'), $lte:ISODate('").Append(date2).Append("')}").
                    Append(",ApplicationId: {$eq:'").Append(applicationId).Append("'} }");                 
            }
            else if(startDate!=null && endingDate != null)
            {
                var date1 = ((DateTime)startDate).Date;
                var date2 = ((DateTime)endingDate).Date.AddDays(1);               

                filterBuilder.Append("{ LoggingDate:{$gte:ISODate('").
                    Append(DateUtilities.FormatDateToIsoFormat(date1)).
                   Append("'), $lte:ISODate('").Append(DateUtilities.FormatDateToIsoFormat(date2)).Append("')} }");
            }
            else if(startDate!=null && !string.IsNullOrWhiteSpace(applicationId))
            {
                var date1 = ((DateTime)startDate).Date;
                var date2 = date1.AddDays(1);               

                filterBuilder.Append("{ LoggingDate:{$gte:ISODate('").
                    Append(DateUtilities.FormatDateToIsoFormat(date1)).
                   Append("'), $lte:ISODate('").Append(DateUtilities.FormatDateToIsoFormat(date2)).Append("')}").
                   Append(",ApplicationId: {$eq:'").Append(applicationId).Append("'} }");
            }
            else if(endingDate!=null && !string.IsNullOrWhiteSpace(applicationId))
            {
                var date1 = ((DateTime)endingDate).Date;
                var date2 = date1.AddDays(1);

                filterBuilder.Append("{ LoggingDate:{$gte:ISODate('").
                   Append(DateUtilities.FormatDateToIsoFormat(date1)).
                   Append("'), $lte:ISODate('").Append(DateUtilities.FormatDateToIsoFormat(date2)).Append("')}").
                   Append(",ApplicationId: {$eq:'").Append(applicationId).Append("'} }");                
            }
            else if (startDate != null)
            {
                var date1 = ((DateTime)startDate).Date;
                var date2 = date1.AddDays(1);

                filterBuilder.Append("{ LoggingDate:{$gte:ISODate('").
                    Append(DateUtilities.FormatDateToIsoFormat(date1)).
                 Append("'), $lte:ISODate('").
                 Append(DateUtilities.FormatDateToIsoFormat(date2)).Append("')} }");              
            }
            else if (endingDate != null)
            {
                var date1 = ((DateTime)endingDate).Date;
                var date2 = date1.AddDays(1);

                string d1 = DateUtilities.FormatDateToIsoFormat(date1),
                    d2 = DateUtilities.FormatDateToIsoFormat(date2);

                filterBuilder.Append("{ LoggingDate:{$gte:ISODate('").Append(d1).
                Append("'), $lte:ISODate('").Append(d2).Append("')} }");                
            }
            else if (!string.IsNullOrWhiteSpace(applicationId))
            {                              
                 filterBuilder.Append("{ApplicationId: {$eq:'").Append(applicationId).Append("'}}");
            }
            else
            {
                return logs;
            }           

            var results = MongoDbProvider.GetHunterLogsCollection().
                        Find(filterBuilder.ToString()).ToList();

            results.ForEach(log =>
            {
                logs.Add(GetLogPayloadEntity(log));
            });

            return logs.OrderBy(log=>log.LoggingDate).ToList();
        }

        public List<LogPayloadEntity> FilterLogs(string json)
        {
            var results = MongoDbProvider.GetHunterLogsCollection().Find(json).Limit(1000).ToList();
            var logs = new List<LogPayloadEntity>();

            results.ForEach(log =>
            {
                logs.Add(GetLogPayloadEntity(log));
            });

            return logs;
        }

        public string ConstructQueryFilters(DateTime? startDate,DateTime? endingDate,
           string applicationId)
        {
            var filterBuilder = new StringBuilder();
            if(startDate!=null && endingDate != null)
            {
                var date1 = ((DateTime)startDate).ToString("yyyy-MM-dd");
                var date2= ((DateTime)endingDate).AddDays(1).ToString("yyyy-MM-dd");
                filterBuilder.Append("{ LoggingDate:{$gte:ISODate('").Append(date1).
                    Append("'), $lte:ISODate('").Append(date2).Append("')}");
            }
            else if (startDate != null)
            {
                var date1 = ((DateTime)startDate).ToString("yyyy-MM-dd");
                var date2 = ((DateTime)startDate).Date.AddDays(1).ToString("yyyy-MM-dd");

                filterBuilder.Append("{ LoggingDate:{$gte:ISODate('").Append(date1).
                    Append("'), $lte:ISODate('").Append(date2).Append("')}");
            }
            else if (endingDate != null)
            {
                var date1 = ((DateTime)endingDate).ToString("yyyy-MM-dd");
                var date2 = ((DateTime)endingDate).Date.AddDays(1).ToString("yyyy-MM-dd");

                filterBuilder.Append("{ LoggingDate:{$gte:ISODate('").Append(date1).
                    Append("'), $lte:ISODate('").Append(date2).Append("')}");
            }          

            return filterBuilder.Append("}").ToString();
        }

        public List<LogPayloadEntity> GetLatestLogs()
        {
            var logEntities = new List<LogPayloadEntity>();
            var matchingLogs = MongoDbProvider.GetHunterLogsCollection().Find(new BsonDocument())
                .Limit(100).SortByDescending(pl => pl.LoggingDate).ToList();

            matchingLogs.ForEach(pl =>
            {
                logEntities.Add(GetLogPayloadEntity(pl));
            });

            return logEntities.OrderBy(le=>le.LoggingDate).ToList();
        }

        public LogPayloadEntity GetLogPayloadEntity(LogPayload logPayload)
        {
            if (logPayload == null)
                throw new ArgumentNullException("logPayLoad cannot be null");

            if (logPayload.LoggingSource == LogSource.AppLogger)
            {
                logPayload = JsonConvert.DeserializeObject<LogPayload>(logPayload.LogMessage);
            }

            if(logPayload.Runtime!=null && logPayload.Runtime.ToLower().Equals(".net"))
            {
                logPayload.LoggingDate = logPayload.LoggingDate.ToLocalTime();
            }

            return new LogPayloadEntity
            {
                ApplicationId = logPayload.ApplicationId,
                Category = logPayload.Category == null ? string.Empty : logPayload.Category,
                Subcategory = logPayload.Subcategory == null ? string.Empty : logPayload.Subcategory,
                LoggingDate = logPayload.LoggingDate.ToString("dd-MM-yyyy hh:mm:ss tt"),
                LogLevel = GetLogLevel(logPayload.LogCategorization).ToUpper(),
                LogMessage = logPayload.LogMessage,
                CpuUtilization = logPayload.CpuUtilization == null ? string.Empty : logPayload.CpuUtilization,
                Exception = logPayload.Exception == null ? string.Empty : logPayload.Exception.ToString(),
                IpAddress = logPayload.IpAddress == null ? string.Empty : logPayload.IpAddress,
                MemoryUtilization =logPayload.MemoryUtilization==null?string.Empty:logPayload.MemoryUtilization,
                Options=logPayload.Options,
                OS=logPayload.OS==null?string.Empty:logPayload.OS
            };
        }

        private string GetLogLevel(LogConstants logLevel)
        {
            switch (logLevel)
            {
                case LogConstants.Error:
                    return "error";
                case LogConstants.Info:
                    return "info";
                case LogConstants.Critical:
                    return "critical";
                case LogConstants.Warning:
                    return "warning";
                default: return "unknown";
            }
        }

        public async Task<List<LogPayloadEntity>> GetNewLogs()
        {
            var logEntities = new List<LogPayloadEntity>();

            try
            {
                var latestLogsTimeStamp = GetLatestDate();

                var logsMatched = new List<LogPayload>();

                if (latestLogsTimeStamp == null)
                {
                    MongoDbProvider.GetLatestDateCollection().InsertOne(new LatestDateEntity
                    {
                        LatestDate= DateTime.Now
                    });

                    logsMatched = MongoDbProvider.GetHunterLogsCollection().Find(new BsonDocument()).Limit(100).ToList();
                }
                else
                {
                    var filter = new FilterDefinitionBuilder<LogPayload>().Gt(payload => payload.LoggingDate,
                        latestLogsTimeStamp.LatestDate);

                    logsMatched = MongoDbProvider.GetHunterLogsCollection().Find(filter).Limit(100).ToList();

                    latestLogsTimeStamp.LatestDate = DateTime.Now;

                    MongoDbProvider.GetLatestDateCollection().UpdateOne(
                        Builders<LatestDateEntity>.Filter.Eq("_id", latestLogsTimeStamp._id),
                        Builders<LatestDateEntity>.Update.Set("LatestDate",DateTime.Now));

                    latestLogsTimeStamp = GetLatestDate();
                }                

                logsMatched.ForEach(payLoad =>
                {
                    logEntities.Add(GetLogPayloadEntity(payLoad));
                });               
            }
            catch(Exception ex)
            {
                Log.Error(ex, "An error occured getting logs");
            }
           

            return logEntities;
        }

        public LatestDateEntity GetLatestDate()
        {
            return MongoDbProvider.GetLatestDateCollection().
                        Find(new BsonDocument()).FirstOrDefault();
        }
    }
}