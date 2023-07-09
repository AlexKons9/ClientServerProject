using RabbitMQ.Client;
using RabbitMQ.RPC.Handler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.RPC.Handler
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;

        public ConnectionProvider(string url)
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri(url)
            };

            _connection = _factory.CreateConnection();
        }

        public IConnection GetConnection()
        {
            return _connection;
        }
    }
}
