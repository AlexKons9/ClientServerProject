using RabbitMQ.RPC.Handler.Interfaces;
using RabbitMQ.RPC.Handler;
using Microsoft.Azure.Cosmos;
using ServerApp.RequesrProcessors;
using ServerApp.Services;
using ServerApp.Services.Interfaces;
using ServerApp.RequestProcessors;

namespace ServerApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize DB 
            var _context = new CosmosDBContext();

            // Initialize all services we need with context
            ILoggingService logger = new LoggingService(_context);
            IUserService userService = new UserService(_context);

            // Create a list of the processes that the server will do
            List<IRequestProcessor> requestProcessors = new List<IRequestProcessor>
            {
                new UserLoginRequestProcessor(logger,userService),
                new MathRequestProcessor(logger),
                new FetchLogsRequestProcessor(logger),
            };

            // Connect to the message broker
            IConnectionProvider connectionProvider = new ConnectionProvider("amqp://guest:guest@localhost:5672");
            // Create Server
            IRPCServer rpcServer = new RPCServer(connectionProvider, requestProcessors);

            Console.WriteLine("RPC Server started. Listening for requests...");
            rpcServer.StartListening();
            Console.WriteLine("Press any key to stop the server.");
            Console.ReadKey();
            rpcServer.StopListening();
        }
    }
}