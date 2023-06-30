using Microsoft.Azure.Cosmos;
using ServerApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Services
{
    public class UserService : IUserService
    {
        private CosmosDBContext _context;

        public UserService(CosmosDBContext context)
        {
            _context = context;
        }

        public async Task InsertUser(Models.User user)
        {
            await Console.Out.WriteLineAsync("Creating User...");

            try
            {
                ItemRequestOptions itemRequestOptions = new ItemRequestOptions
                {
                    EnableContentResponseOnWrite = false
                };


                var response =  await _context.userContainer.CreateItemAsync<Models.User>(user, new PartitionKey(user.UserName), itemRequestOptions);


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

        public async Task<Models.User> RetrieveSingleUser(string username, string password)
        {
            try
            {
                var query = $"SELECT * FROM UserContainer WHERE UserContainer.username = \"{username}\" AND UserContainer.password = \"{password}\"";
                var iterator = _context.userContainer.GetItemQueryIterator<Models.User>(query);
                var res = await iterator.ReadNextAsync();
                return res.ToList().First();
            }
            catch (Exception ex)
            {

                throw new Exception($"Something on retrieving signle user went wrong... " +
                    $"Exception: {ex.Message}");
            }
        }
    }
}
