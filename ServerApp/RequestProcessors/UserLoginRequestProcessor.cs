using Models.Enums;
using RabbitMQ.RPC.Handler.Interfaces;
using ServerApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.RequesrProcessors
{
    public class UserLoginRequestProcessor : IRequestProcessor
    {

        private ILoggingService _loggingService;
        private IUserService _userService;

        public GeneralTypeOfRequest Type { get; } = GeneralTypeOfRequest.UserLogin;

        public UserLoginRequestProcessor(ILoggingService loggingService, IUserService userService)
        {
            _loggingService = loggingService;
            _userService = userService;
        }

        public object ProcessRequest(object request)
        {
            throw new NotImplementedException();
        }
    }
}
