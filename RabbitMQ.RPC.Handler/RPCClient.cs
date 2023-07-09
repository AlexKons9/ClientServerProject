using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.RPC.Handler.Interfaces;
using Newtonsoft.Json;

namespace RabbitMQ.RPC.Handler
{
    public class RPCClient : IRPCClient, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IConnectionProvider _connectionProvider;
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;
        private TaskCompletionSource<string> _responseReceived;

        public RPCClient(IConnectionProvider connectionProvider)
        {
            // URI 
            // "amqp://guest:guest@localhost:5672"

            _connectionProvider = connectionProvider;
            _connection = _connectionProvider.GetConnection();
            _channel = _connection.CreateModel();

            _replyQueueName = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);
            _responseReceived = new TaskCompletionSource<string>();

            _consumer.Received += (sender, args) =>
            {
                if (args.BasicProperties.CorrelationId == _responseReceived.Task.Id.ToString())
                {
                    var body = args.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);
                    _responseReceived.TrySetResult(response);
                }
            };
        }

        public string SendRequest(object request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var replyQueueName = _channel.QueueDeclare().QueueName;

            var props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));

            var responseReceived = new TaskCompletionSource<string>();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                if (args.BasicProperties.CorrelationId == correlationId)
                {
                    var response = Encoding.UTF8.GetString(args.Body.ToArray());
                    responseReceived.SetResult(response);
                }
            };

            _channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            _channel.BasicPublish(
                exchange: "",
                routingKey: "rpc_queue",
                basicProperties: props,
                body: messageBytes);

            return responseReceived.Task.GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
