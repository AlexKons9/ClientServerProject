using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using RabbitMQ.RPC.Handler;
using RabbitMQ.RPC.Handler.Interfaces;
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

        [HttpPost]
        public async Task<ActionResult> Post(MathRequest request)
        {
            var requestBus = new RequestHandler<MathRequest>(request, Models.Enums.GeneralTypeOfRequest.MathCalculation);
            var response = await Task.FromResult(_client.SendRequest(requestBus));
            return Ok(response);
        }

        //[HttpGet]
        //public async Task<ActionResult> Login(string username, string password)
        //{
        //    var requestBus = new RequestHandler<User>(new { }, Models.Enums.GeneralTypeOfRequest.UserLogin);
        //    var response = await Task.FromResult(_client.SendRequest(requestBus));
        //    return Ok(response);
        //}
    }
}
