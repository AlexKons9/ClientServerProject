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
    public class CosmosDBHandler
    {
        private const string AccountEndpoint = "https://localhost:8081";
        private const string AccountKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private const string DatabaseName = "ProjectDB";
        private const string LoggingContainer = "LoggingContainer";
        private const string UserContainer = "UserContainer";

        private static CosmosClient cosmosClient;
        private static Database database;
        private static Microsoft.Azure.Cosmos.Container userContainer;
        private static Microsoft.Azure.Cosmos.Container logContainer;

        //static async Task Main(string[] args)
        //{
        //    cosmosClient = CreateConnection();
        //    await InitializeDatabaseAndContainer();
        //    await InsertUser();
        //    await RetrieveUser();

        //    Console.WriteLine("Press any key to exit...");
        //    Console.ReadKey();
        //}
        
        public static async Task InitializeHandler()
        {
            cosmosClient = CreateConnection();
            await InitializeDatabaseAndContainer();
        }


        public static CosmosClient CreateConnection()
        {
            CosmosClientBuilder cosmosClientBuilder = new CosmosClientBuilder(AccountEndpoint, AccountKey)
                .WithConnectionModeDirect().WithBulkExecution(true).WithSerializerOptions(new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.Default
                });
            return cosmosClientBuilder.Build();
        }

        public static async Task InitializeDatabaseAndContainer()
        {
            database = await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseName);
            userContainer = await database.CreateContainerIfNotExistsAsync(UserContainer, "/username");
            logContainer = await database.CreateContainerIfNotExistsAsync(LoggingContainer, "/id");
        }



        // Methods for user

        public static async Task InsertUser(Models.User user)
        {
            await Console.Out.WriteLineAsync( "Creating User...");

            try
            {
                ItemRequestOptions itemRequestOptions = new ItemRequestOptions
                {
                    EnableContentResponseOnWrite = false
                };


                var response = await userContainer.CreateItemAsync<Models.User>(user, new PartitionKey(user.UserName), itemRequestOptions);


                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    Console.WriteLine("User inserted successfully.");
                }
                else
                {
                    Console.WriteLine("An error occurred while inserting user: " + response.StatusCode);
                }
            }
            catch (CosmosException ex) // Catch CosmosException specifically
            {
                Console.WriteLine("CosmosException: " + ex.Message);
                //Console.WriteLine("Diagnostics: " + ex.Diagnostics.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<Models.User> RetrieveSingleUser(string username, string password)
        {
            try
            {
                var query = $"SELECT * FROM UserContainer WHERE UserContainer.username = \"{username}\" AND UserContainer.password = \"{password}\"";
                var iterator = userContainer.GetItemQueryIterator<Models.User>(query);
                var res = await iterator.ReadNextAsync();
                return res.ToList().First();
            }
            catch (Exception ex)
            {

                throw new Exception("Something on retrieving signle user went wrong...");
            }
        }


        // Methods for logging

        public static async Task InsertLog(Log log)
        {
            await Console.Out.WriteLineAsync("Importing log...");

            try
            {
                ItemRequestOptions itemRequestOptions = new ItemRequestOptions
                {
                    EnableContentResponseOnWrite = false
                };


                var response = await logContainer.CreateItemAsync<Log>(log, new PartitionKey(log.Id), itemRequestOptions);


                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    Console.WriteLine("Log inserted successfully.");
                }
                else
                {
                    Console.WriteLine("An error occurred while inserting log " + response.StatusCode);
                }
            }
            catch (CosmosException ex) // Catch CosmosException specifically
            {
                Console.WriteLine("CosmosException: " + ex.Message);
                //Console.WriteLine("Diagnostics: " + ex.Diagnostics.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<IList<Log>> RetrieveLogsBetweenTimeSpan(DateTime Start, DateTime Stop)
        {
            var query = $"SELECT * FROM LoggingContainer WHERE ";
            var iterator = logContainer.GetItemQueryIterator<Log>(query);
            var res = await iterator.ReadNextAsync();
            return res.ToList();
        }
    }
}
