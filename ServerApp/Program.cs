using RabbitMQ.RPC.Handler.Interfaces;
using RabbitMQ.RPC.Handler;
using Microsoft.Azure.Cosmos;

namespace ServerApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //await CosmosDBHandler.InitializeHandler();

            //var user = new Models.User
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    UserName = "Kokos",
            //    Password = "Rikos"
            //};

            ////await CosmosDBHandler.InsertUser(user);
            //var res = await CosmosDBHandler.RetrieveSingleUser("Kokos", "Rikos");

            //Console.WriteLine(res.UserName);

            IConnectionProvider connectionProvider = new ConnectionProvider("amqp://guest:guest@localhost:5672");
            IRPCServer rpcServer = new RPCServer(connectionProvider);

            Console.WriteLine("RPC Server started. Listening for requests...");
            rpcServer.StartListening();
            Console.WriteLine("Press any key to stop the server.");
            Console.ReadKey();
            rpcServer.StopListening();
        }
    }
}