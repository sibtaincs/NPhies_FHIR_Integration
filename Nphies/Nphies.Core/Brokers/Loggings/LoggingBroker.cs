using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Nphies.Core.Extensions;
using Serilog;



namespace Nphies.Core.Brokers.Loggings
{
    public class LoggingBroker : ILoggingBroker
    {
        private readonly ILogger logger;
        public LoggingBroker(IConfiguration configuration)
        {
            var mongoConnectionString = configuration.GetSection("ConnectionStrings:MongodbConnection")
                                  .Value.Split(new string[] { "//" },
                                  StringSplitOptions.None);
            var constr = mongoConnectionString[0] + "//" + configuration["Mongo_UserName"]
                                    + ":" + configuration["Mongo_Password"]
                                    + "@" + mongoConnectionString[1];
           
          
            string mongoDbUserName = configuration["Mongo_UserName"];
            string mongoDbPassword = configuration["Mongo_Password"];

            this.logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/NphiesApi-log.txt", rollingInterval: Serilog.RollingInterval.Day)
                .WriteTo.MongoDBBson(configuration =>
                {
                    var mongodbSetting = new MongoClientSettings
                    {
                        UseTls = true,
                        AllowInsecureTls = true,
                        Credential = MongoCredential.CreateCredential("NphiesApiErrorLogs", mongoDbUserName, mongoDbPassword),
                        Server = new MongoServerAddress(mongoConnectionString[1].Split(":")[0])
                    };
                    var mongoDbInstance = new MongoClient(mongodbSetting).GetDatabase("NphiesApiErrorLogs");

                    configuration.SetMongoDatabase(mongoDbInstance);
                    configuration.SetRollingInternal((Serilog.Sinks.MongoDB.RollingInterval)RollingInterval.Month);
                })
                .CreateLogger();


        }
        public void LogCritical(Exception exception)=>
             this.logger.Error(exception, $"{exception.Message} {exception.GetValidationSummary()}");

        public void LogError(string message)=>
              this.logger.Error(message);

        public void LogInfo(string message) =>
              this.logger.Information(message);

        public void LogWarning(string message) =>
              this.logger.Warning(message);

        public void LogWarning(Exception exception)=>
             this.logger.Error(exception, $"{exception.Message} {exception.GetValidationSummary()}");
    }
}
