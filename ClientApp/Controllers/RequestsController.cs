using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Post(Request request)
        {
            var response =  _client.SendRequest(request);
            return Ok(response);
        }
    }
}
