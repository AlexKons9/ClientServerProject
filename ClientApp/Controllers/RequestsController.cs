using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Newtonsoft.Json;
using RabbitMQ.RPC.Handler;
using RabbitMQ.RPC.Handler.Interfaces;
using ServerApp;
using System.Net;
using System.Net.Sockets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClientApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        IConnectionProvider _connectionProvider;
        IRPCClient _client;

        public RequestsController(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _client = new RPCClient(_connectionProvider);
        }

        [HttpPost("Math")]
        public async Task<ActionResult> Math(MathRequestDTO request)
        {
            string ip = await GetIp();
            request.LogInfo.IP = ip;
            var requestBus = new RequestHandler<MathRequestDTO>(request, Models.Enums.GeneralTypeOfRequest.MathCalculation);
            var response = await Task.FromResult(_client.SendRequest(requestBus));
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(User user)
        {
            string ip = await GetIp();
            var logInfo = new LogInfoDTO { UserName = user.UserName, IP = ip };
            var userDTO = new UserDTO { User = user, LogInfo = logInfo };

            var requestBus = new RequestHandler<UserDTO>(userDTO, Models.Enums.GeneralTypeOfRequest.UserLogin);
            var response = await Task.FromResult(_client.SendRequest(requestBus));
            return Ok(response);
        }

        [HttpPost("Logs")]
        public async Task<ActionResult> GetLogs(FetchLogsDTO data)
        {
            string ip = await GetIp();
            data.LogInfo.IP = ip;   
            var requestBus = new RequestHandler<FetchLogsDTO>(data, Models.Enums.GeneralTypeOfRequest.FetchLogs);
            var response = await Task.FromResult(_client.SendRequest(requestBus));
            return Ok(response);
        }


        // Gets the IP of user
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<string?> GetIp()
        {
            string? ip = Response.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip == "::1" || ip == "127.0.0.1")
            {
                ip = Dns.GetHostEntry(Dns.GetHostName())
                        .AddressList
                        .FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        ?.ToString();
            }

            return await Task.FromResult(ip);
        }

    }
}
