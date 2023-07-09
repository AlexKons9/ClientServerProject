using Microsoft.Azure.Cosmos;
using ServerApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Services
{
    public class LoggingService : ILoggingService
    {
        private CosmosDBContext _context;

        public LoggingService(CosmosDBContext context)
        {
            _context = context;
        }

        

        public async Task InsertLog(Log log)
        {
            await Console.Out.WriteLineAsync("Importing log...");

            try
            {
                ItemRequestOptions itemRequestOptions = new ItemRequestOptions
                {
                    EnableContentResponseOnWrite = false
                };


                var response = await _context.logContainer.CreateItemAsync<Log>(log, new PartitionKey(log.Id), itemRequestOptions);


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

        public async Task<IEnumerable<Log>> RetrieveLogsBetweenTimeSpan(DateTime startTime, DateTime endTime)
        {
            string sqlQuery = $"SELECT * FROM c WHERE c.dateoflog >= @startDate AND c.dateoflog <= @endDate";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQuery)
                .WithParameter("@startDate", startTime)
                .WithParameter("@endDate", endTime);


            //var query = $"SELECT * FROM LoggingContainer WHERE LoggingContainer.dateoflog >= \"2023-06-30T16:35:04.0799625+03:00\" " +
            //    $"AND LoggingContainer.dateoflog <= \"2023-06-30T18:18:04.9781537+03:00\"";
            var iterator = _context.logContainer.GetItemQueryIterator<Log>(queryDefinition);
            var res = await iterator.ReadNextAsync();   
            return res.ToList();
        }
    }
}
