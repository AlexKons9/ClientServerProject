using ClientApp;
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

namespace ServerApp.RequesrProcessors
{
    public class MathRequestProcessor : IRequestProcessor
    {
        private ILoggingService _loggingService;

        public GeneralTypeOfRequest Type { get; } = GeneralTypeOfRequest.MathCalculation;

        public MathRequestProcessor(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public async Task<object> ProcessRequest(object request)
        {
            var req = JsonConvert.DeserializeObject<MathRequestDTO>(request.ToString());
            var mathReq = req.MathRequest;
            var logInfo = req.LogInfo;
            
            if (logInfo != null) { }


            var requestType = mathReq.ParseRequestType(mathReq.TypeOfRequest);
            var num1 = mathReq.Numbers[0];
            var num2 = mathReq.Numbers[1];

            await _loggingService.InsertLog(new Log()
            {
                Id = Guid.NewGuid().ToString(),
                Details = $"Attemting to {mathReq.TypeOfRequest} - number: {num1} and number: {num2}",
                DateOfLog = DateTime.Now,
                UserName = logInfo.UserName,
                IP = logInfo.IP
            });

            try
            {
                switch (requestType)
                {
                    case MathRequestType.Add:
                        mathReq.Result = Calculator.Add(num1, num2);
                        break;
                    case MathRequestType.Subtract:
                        mathReq.Result = Calculator.Subtract(num1, num2);
                        break;
                    case MathRequestType.Multiply:
                        mathReq.Result = Calculator.Multiply(num1, num2);
                        break;
                    case MathRequestType.Divide:
                        mathReq.Result = Calculator.Divide(num1, num2);
                        break;
                    default: throw new ArgumentException("Wrong type of request given.");
                }
            }
            catch (ArgumentException ex)
            {
                await _loggingService.InsertLog(new Log()
                {
                    Id = Guid.NewGuid().ToString(),
                    Details = $"Error: {ex.Message}",
                    DateOfLog = DateTime.Now,
                    UserName = logInfo.UserName,
                    IP = logInfo.IP
                });

                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.InsertLog(new Log()
                {
                    Id = Guid.NewGuid().ToString(),
                    Details = $"Error: {ex.Message}",
                    DateOfLog = DateTime.Now,
                    UserName = logInfo.UserName,
                    IP = logInfo.IP
                });
                throw new Exception(ex.Message);
            }

            await _loggingService.InsertLog(new Log()
            {
                Id = Guid.NewGuid().ToString(),
                Details = $"Success of Calculation... The result is: {mathReq.Result}",
                DateOfLog = DateTime.Now,
                UserName = logInfo.UserName,
                IP = logInfo.IP
            });

            return req;
        }
    }
}
