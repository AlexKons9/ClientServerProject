using ClientApp;
using Microsoft.Azure.Cosmos;
using Models;
using Models.DTO;
using Models.Enums;
using Newtonsoft.Json;
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

        public async Task<object> ProcessRequest(object request)
        {
            var userDTO = JsonConvert.DeserializeObject<UserDTO>(request.ToString());
            //var logInfo = JsonConvert.DeserializeObject<LogInfoDTO>(userDTO.LogInfo.ToString());

            var userIP = userDTO.LogInfo.IP;
            var user = userDTO.User;

            var username = user.UserName;
            var password = user.Password;
            await _loggingService.InsertLog(new Log()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = username,
                Details = $"Attemting to Login - username: {username}",
                DateOfLog = DateTime.Now,
                IP = userIP
            });

            try
            {
                var dbUser = await _userService.RetrieveSingleUser(username, password);

                if (dbUser == null)
                {
                    return "Fail";
                }

                await _loggingService.InsertLog(new Log()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = username,
                    Details = $"Logged in Successfully!",
                    DateOfLog = DateTime.Now,
                    IP = userIP
                });

                return dbUser;

            }
            catch (Exception ex)
            {
                await _loggingService.InsertLog(new Log()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = username,
                    Details = ex.Message,
                    DateOfLog = DateTime.Now,
                    IP = userIP
                });
                throw new Exception(ex.Message);
            }
        }
    }
}
