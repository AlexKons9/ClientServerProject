using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.RPC.Handler.Interfaces;
using System;
using System.Reflection.Metadata;
using System.Text;
using ClientApp;
using Models.Enums;
using Models;

namespace RabbitMQ.RPC.Handler
{
    public class RPCServer : IRPCServer
    {
        //private readonly IConnectionProvider _connectionProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly List<IRequestProcessor> _requestProcessors;

        public RPCServer(IConnectionProvider connectionProvider, List<IRequestProcessor> requestProcessores)
        {
            // URI
            //_connectionProvider = new ConnectionProvider("amqp://guest:guest@localhost:5672");
            //_connection = _connectionProvider.GetConnection();

            //_connectionProvider = connectionProvider;
            _connection = connectionProvider.GetConnection();
            _channel = _connection.CreateModel();
            _requestProcessors = requestProcessores;

            _channel.QueueDeclare(
                queue: "rpc_queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }

        public void StartListening()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                object response = null;

                var body = args.Body.ToArray();
                var request = JsonConvert.DeserializeObject<RequestHandler<object>>(Encoding.UTF8.GetString(body));
                var correlationId = args.BasicProperties.CorrelationId;

                try
                {
                    // Process the request and generate the response
                    response =  ProcessRequest(request);
                }
                catch (Exception ex)
                {
                    // Handle any errors or exceptions
                    response = "Error: " + ex.Message;
                }
                finally
                {
                    var replyProperties = _channel.CreateBasicProperties();
                    replyProperties.CorrelationId = correlationId;

                    var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                    
                    _channel.BasicPublish(
                        exchange: "",
                        routingKey: args.BasicProperties.ReplyTo,
                        basicProperties: replyProperties,
                        body: responseBytes);

                    _channel.BasicAck(
                        deliveryTag: args.DeliveryTag,
                        multiple: false);
                }
            };
            _channel.BasicConsume(
                queue: "rpc_queue",
                autoAck: false,
                consumer: consumer);

        }

        public void StopListening()
        {
            _channel.Close();
            _connection.Close();
        }



        private object ProcessRequest(RequestHandler<object> requestHandler)
        {
            // Find the appropriate IRequestProcessor based on the request type
            IRequestProcessor requestProcessor = FindRequestProcessor(requestHandler.Type);

            if (requestProcessor != null)
            {
                try
                {
                    // Invoke the ProcessRequest method of the found processor
                    var response = requestProcessor
                        .ProcessRequest(requestHandler.Data)
                        .GetAwaiter()
                        .GetResult();
                    return response;
                }
                catch (Exception ex)
                {
                    // Handle any errors or exceptions
                    return "Error: " + ex.Message;
                }
            }
            else
            {
                // If no request processor is found for the request type, return an error response
                return "Error: No request processor found for the request type.";
            }


            //var requestType = request.ParseRequestType(request.TypeOfRequest);
            //var num1 = request.Numbers[0];
            //var num2 = request.Numbers[1];

            //switch (requestType)
            //{
            //    case MathRequestType.Add:
            //        request.Result = num1 + num2;
            //        break;
            //    case MathRequestType.Subtract:
            //        request.Result = num1 - num2;
            //        break;
            //    case MathRequestType.Multiply:
            //        request.Result = num1 * num2;
            //        break;
            //    case MathRequestType.Divide:
            //        request.Result = num1 / num2;
            //        break;
            //    default:
            //        break;
            //}

            //return request;
        }


        private IRequestProcessor FindRequestProcessor(GeneralTypeOfRequest requestType)
        {
            try
            {
                // Iterate through the collection of IRequestProcessors and find the one that can handle the request type
                foreach (IRequestProcessor requestProcessor in _requestProcessors)
                {
                    if (requestProcessor.Type == requestType)
                    {
                        return requestProcessor;
                    }
                }

                return null; // Return null if no matching request processor is found
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
