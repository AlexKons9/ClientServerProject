using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public class CosmosDBContext
    {
        private const string AccountEndpoint = "https://localhost:8081";
        private const string AccountKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private const string DatabaseName = "ProjectDB";
        private const string LoggingContainer = "LoggingContainer";
        private const string UserContainer = "UserContainer";

        private CosmosClient cosmosClient;
        private Database database;
        public Microsoft.Azure.Cosmos.Container userContainer;
        public Microsoft.Azure.Cosmos.Container logContainer;

        public CosmosDBContext()
        {
            cosmosClient = CreateConnection();
            InitializeDatabaseAndContainer().GetAwaiter().GetResult();
        }


        public CosmosClient CreateConnection()
        {
            CosmosClientBuilder cosmosClientBuilder = new CosmosClientBuilder(AccountEndpoint, AccountKey)
                .WithConnectionModeDirect().WithBulkExecution(true).WithSerializerOptions(new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.Default
                });
            return cosmosClientBuilder.Build();
        }

        public async Task InitializeDatabaseAndContainer()
        {
            database = await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseName);
            userContainer = await database.CreateContainerIfNotExistsAsync(UserContainer, "/username");
            logContainer = await database.CreateContainerIfNotExistsAsync(LoggingContainer, "/id");
        }
    }
}
