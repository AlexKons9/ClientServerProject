using ClientApp;
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
            var req = JsonConvert.DeserializeObject<MathRequest>(request.ToString());

            var requestType = req.ParseRequestType(req.TypeOfRequest);
            var num1 = req.Numbers[0];
            var num2 = req.Numbers[1];

            await _loggingService.InsertLog(new Log()
            {
                Id = Guid.NewGuid().ToString(),
                Details = $"Attemting to {req.TypeOfRequest} - number: {num1} and number: {num2}",
                DateOfLog = DateTime.Now,
            });

            try
            {
                switch (requestType)
                {
                    case MathRequestType.Add:
                        req.Result = Calculator.Add(num1, num2);
                        break;
                    case MathRequestType.Subtract:
                        req.Result = Calculator.Subtract(num1, num2);
                        break;
                    case MathRequestType.Multiply:
                        req.Result = Calculator.Multiply(num1, num2);
                        break;
                    case MathRequestType.Divide:
                        req.Result = Calculator.Divide(num1, num2);
                        break;
                    default: throw new ArgumentException("Wrong type of request given.");
                }
            }
            catch (ArgumentException ex)
            {

                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            await _loggingService.InsertLog(new Log()
            {
                Id = Guid.NewGuid().ToString(),
                Details = $"Success of Calculation... The result is: {req.Result}",
                DateOfLog = DateTime.Now,
            });

            return req;
        }
    }
}
