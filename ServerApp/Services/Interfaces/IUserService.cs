using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Services.Interfaces
{
    public interface IUserService
    {
        Task InsertUser(Models.User user);
        Task<Models.User> RetrieveSingleUser(string username, string password);
    }
}
