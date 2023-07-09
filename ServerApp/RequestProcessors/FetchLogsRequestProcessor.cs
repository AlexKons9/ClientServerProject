using Models.DTO;
using Models.Enums;
using Newtonsoft.Json;
using RabbitMQ.RPC.Handler.Interfaces;
using ServerApp.Services;
using ServerApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.RequestProcessors
{
    public class FetchLogsRequestProcessor : IRequestProcessor
    {
        private ILoggingService _loggingService;

        public GeneralTypeOfRequest Type { get; } = GeneralTypeOfRequest.FetchLogs;


        public FetchLogsRequestProcessor(ILoggingService loggingService)
        {
           _loggingService = loggingService;
        }


        public async Task<object> ProcessRequest(object request)
        {
            var timeSpan = JsonConvert.DeserializeObject<FetchLogsDTO>(request.ToString());
            var logInfo = timeSpan.LogInfo;

            await _loggingService.InsertLog(new Log()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = logInfo.UserName,
                Details = $"User: {logInfo.UserName} tries to fetch logs....",
                DateOfLog = DateTime.Now,
                IP = logInfo.IP
            });

            try
            {
                var logs = await  _loggingService.RetrieveLogsBetweenTimeSpan(timeSpan.StartTime, timeSpan.EndTime);

                await _loggingService.InsertLog(new Log()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = logInfo.UserName,
                    Details = $"User: {logInfo.UserName} fetched logs successfully....",
                    DateOfLog = DateTime.Now,
                    IP = logInfo.IP
                });

                return logs;

            }
            catch (Exception ex)
            {
                await _loggingService.InsertLog(new Log()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = logInfo.UserName,
                    Details = ex.Message,
                    DateOfLog = DateTime.Now,
                    IP = logInfo.IP
                });
                throw new Exception(ex.Message);
            }

        }
    }
}
