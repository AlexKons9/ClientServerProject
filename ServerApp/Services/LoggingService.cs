using Microsoft.Azure.Cosmos;
using ServerApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IList<Log>> RetrieveLogsBetweenTimeSpan(DateTime Start, DateTime Stop)
        {
            var query = $"SELECT * FROM LoggingContainer WHERE LoggingContainer.dateoflog > \"{Start}\" AND LoggingContainer.dateoflog > \"{Stop}\"";
            var iterator = _context.logContainer.GetItemQueryIterator<Log>(query);
            var res = await iterator.ReadNextAsync();
            return res.ToList();
        }
    }
}
